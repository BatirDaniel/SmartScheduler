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

        /// <summary>
        ///  Metodă principală: Găsește o "ordonare" a sarcinilor care minimizează 
        ///  costul total = (RequiredHours - bonusHobby).
        ///  - Bonus: Dacă userul are un hobby = task.Category, scadem 1 oră din cost (minim 0).
        ///  
        ///  Returnează lista de TaskModel în ordinea în care A* le "planifică".
        /// </summary>
        /// <param name="allTasks">Sarcinile de planificat (ex. toate tasks din DB)</param>
        /// <param name="user">Utilizatorul curent (pentru a vedea hobby-urile)</param>
        /// <returns>O listă de TaskModel în ordinea optimă găsită</returns>
        public List<TaskModel> FindOptimalTaskOrderWithHobby(
             List<TaskModel> allTasks, User user)
        {
            // nod start
            var start = new AStarNode
            {
                TasksDone = new List<TaskModel>(),
                GCost = 0,
                HCost = CalculateHeuristic(allTasks, new List<TaskModel>(), user)
            };
            start.CalculateFCost();

            // ---------- PriorityQueue în loc de List + Sort -----------------
            var open = new PriorityQueue<AStarNode, double>();
            open.Enqueue(start, start.FCost);

            var closed = new HashSet<AStarNode>(new AStarNodeComparer());

            while (open.Count > 0)
            {
                var current = open.Dequeue();

                if (current.TasksDone.Count == allTasks.Count)
                    return current.TasksDone;

                closed.Add(current);

                // succesorii
                foreach (var t in allTasks.Where(t => !current.TasksDone.Contains(t)))
                {
                    var newDone = current.TasksDone.Append(t).ToList();

                    var succ = new AStarNode
                    {
                        TasksDone = newDone,
                        GCost = current.GCost + CalculateCost(t, user),
                        HCost = CalculateHeuristic(allTasks, newDone, user),
                        Parent = current
                    };
                    succ.CalculateFCost();

                    if (closed.Contains(succ)) continue;

                    // dacă acelaşi “set” e deja în coadă cu FCost mai mic – îl ignorăm
                    open.Enqueue(succ, succ.FCost);
                }
            }
            return new List<TaskModel>();
        }

        /// <summary>
        ///  Calculează costul real pentru un Task,
        ///  scăzând 1 oră dacă hobby-ul userului se potrivește cu task.Category.
        ///  Minim 0.
        /// </summary>
        private double CalculateCost(TaskModel task, User user)
        {
            double cost = task.RequiredHours;
            if (!string.IsNullOrEmpty(task.Category)
                && user.Hobbies != null
                && user.Hobbies.Any(h =>
                        h.HobbyName.Equals(task.Category, StringComparison.OrdinalIgnoreCase)))
            {
                cost -= 1;
                if (cost < 0) cost = 0;
            }
            return cost;
        }

        /// <summary>
        ///  Calculează euristica (HCost):
        ///  Presupunem că, pentru task-urile încă neefectuate, 
        ///  costul total e "sumă costurilor" (ore - bonus) 
        ///  (adică un best-case scenario).
        /// </summary>
        private double CalculateHeuristic(List<TaskModel> allTasks, List<TaskModel> done, User user)
        {
            // task-urile rămase = allTasks - done
            var remaining = allTasks.Where(t => !done.Contains(t));
            double sum = 0;
            foreach (var task in remaining)
            {
                sum += CalculateCost(task, user);
            }
            return sum;
        }

        #region Clase interne

        /// <summary>
        ///  Comparer pentru a defini "egalitatea" a două noduri:
        ///  - Două noduri sunt egale dacă au aceleași Task-uri finalizate (ignorăm ordinea).
        /// </summary>
        private class AStarNodeComparer : IEqualityComparer<AStarNode>
        {
            public bool Equals(AStarNode? x, AStarNode? y)
            {
                if (x == null || y == null) return false;
                if (x.TasksDone.Count != y.TasksDone.Count) return false;

                // Verificăm setul de ID-uri
                var xIds = x.TasksDone.Select(t => t.Id).OrderBy(id => id).ToArray();
                var yIds = y.TasksDone.Select(t => t.Id).OrderBy(id => id).ToArray();
                return xIds.SequenceEqual(yIds);
            }

            public int GetHashCode(AStarNode obj)
            {
                // Suma ID-urilor e un hash simplu
                int sum = obj.TasksDone.Sum(t => t.Id);
                return sum.GetHashCode();
            }
        }
        #endregion
    }
}
