using SmartScheduler.WPF.Models;
using SmartScheduler.WPF.Models.AlgorithmsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Services.Algorithms
{
    public class AStarService
    {
        private static AStarService? _instance;

        private AStarService() { }

        public static AStarService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AStarService();
            }
            return _instance;
        }

        /*────────────  CONFIG  ────────────*/
        private const int BeamWidth = 200;   // ↔ precizie vs. performanță
        private const double Noise = 0.001; // ↔ varietate < 1 sec
        private readonly Random _rng = new();

        /*────────────  API  ────────────*/
        public List<TaskModel> FindOptimalTaskOrderWithHobby(
            List<TaskModel> allTasks,
            User user)
        {
            if (allTasks is null || allTasks.Count == 0)
                return new();

            // Ordine stabilă → Contains funcționează corect
            allTasks = allTasks.OrderBy(t => t.Id).ToList();

            /* START NODE */
            var start = new AStarNode
            {
                TasksDone = new(),
                GCost = 0,
                HCost = CalculateHeuristic(allTasks, Array.Empty<TaskModel>(), user)
            };
            start.CalculateFCost();

            /* BEAM SEARCH pe niveluri (număr task‑uri completate) */
            var currentLevel = new List<AStarNode> { start };
            AStarNode? bestComplete = null;

            while (currentLevel.Count > 0)
            {
                var nextLevel = new List<AStarNode>();

                foreach (var node in currentLevel)
                {
                    /* Soluție completă? */
                    if (node.TasksDone.Count == allTasks.Count)
                    {
                        if (bestComplete == null ||
                            node.FCost < bestComplete.FCost)
                            bestComplete = node;
                        continue;
                    }

                    /* Generează succesorii – shuffle pentru aleator */
                    var remaining = allTasks
                        .Where(t => !node.TasksDone.Contains(t))
                        .OrderBy(_ => _rng.Next());

                    foreach (var task in remaining)
                    {
                        var withTask = new List<TaskModel>(node.TasksDone) { task };

                        var succ = new AStarNode
                        {
                            TasksDone = withTask,
                            GCost = node.GCost + CalculateCost(task, user),
                            HCost = CalculateHeuristic(allTasks, withTask, user),
                            Parent = node
                        };
                        succ.CalculateFCost();

                        /* Zgomot mic la FCost pentru varietate */
                        succ.FCost += _rng.NextDouble() * Noise;

                        nextLevel.Add(succ);
                    }
                }

                /* Ținem doar cele mai bune BeamWidth noduri */
                nextLevel.Sort((a, b) => a.FCost.CompareTo(b.FCost));
                if (nextLevel.Count > BeamWidth)
                    nextLevel = nextLevel.Take(BeamWidth).ToList();

                currentLevel = nextLevel;
            }

            return bestComplete?.TasksDone ?? new();
        }

        /*────────────  COSTURI & HEURISTICĂ  ────────────*/
        private double CalculateCost(TaskModel task, User user)
        {
            double cost = task.RequiredHours;

            if (!string.IsNullOrWhiteSpace(task.Category) &&
                user?.Hobbies?.Any(h =>
                    h.HobbyName.Equals(task.Category, StringComparison.OrdinalIgnoreCase)) == true)
            {
                cost = Math.Max(0, cost - 1); // bonus hobby
            }

            /* mic zgomot ±Noise/2 pentru diversitate */
            cost += (_rng.NextDouble() - 0.5) * Noise;
            if (cost < 0) cost = 0;
            return cost;
        }

        private double CalculateHeuristic(
            IReadOnlyList<TaskModel> all,
            IReadOnlyCollection<TaskModel> done,
            User user)
        {
            var remaining = all.Where(t => !done.Contains(t));
            double sum = remaining.Sum(t => CalculateCost(t, user));

            /* admisibilă & strict mai mică -->
               scădem epsilon * nrTaskuriRămase */
            sum = Math.Max(0, sum - remaining.Count() * Noise);
            return sum;
        }

        /*────────────  NODE  ────────────*/
        private sealed class AStarNode
        {
            public List<TaskModel> TasksDone { get; init; } = new();
            public double GCost { get; init; }      // cost real acumulat
            public double HCost { get; init; }      // euristică
            public double FCost { get; set; }       // total (G+H(+noise))
            public AStarNode? Parent { get; init; }

            public void CalculateFCost() => FCost = GCost + HCost;
        }
    }
}
