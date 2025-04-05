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
        public List<TaskModel> FindOptimalTaskOrderWithHobby(List<TaskModel> allTasks, User user)
        {
            // 1) Creăm nodul "start": nicio sarcină completată, cost 0
            var startNode = new AStarNode
            {
                TasksDone = new List<TaskModel>(),
                GCost = 0,
                HCost = CalculateHeuristic(allTasks, new List<TaskModel>(), user)
            };
            startNode.CalculateFCost();

            // "openList": noduri care așteaptă să fie explorate
            var openList = new List<AStarNode> { startNode };
            // "closedList": noduri deja vizitate
            var closedList = new HashSet<AStarNode>(new AStarNodeComparer());

            while (openList.Count > 0)
            {
                // 2) Alegem nodul cu FCost minim
                openList.Sort((a, b) => a.FCost.CompareTo(b.FCost));
                var currentNode = openList[0];

                // Dacă am inclus toate sarcinile (TasksDone = allTasks), suntem gata
                if (currentNode.TasksDone.Count == allTasks.Count)
                {
                    // Returnăm ordinea completă
                    return currentNode.TasksDone;
                }

                // Mutăm nodul din openList în closedList
                openList.RemoveAt(0);
                closedList.Add(currentNode);

                // 3) Generăm succesorii: adăugăm câte 1 task nou din cele nefinalizate
                var remainingTasks = allTasks
                    .Where(t => !currentNode.TasksDone.Contains(t))
                    .ToList();

                foreach (var nextTask in remainingTasks)
                {
                    // Creăm noul "TasksDone"
                    var newTasksDone = new List<TaskModel>(currentNode.TasksDone);
                    newTasksDone.Add(nextTask);

                    // Calculăm GCost = cost parcurs până acum + costul nextTask
                    double g = currentNode.GCost + CalculateCost(nextTask, user);

                    var successor = new AStarNode
                    {
                        TasksDone = newTasksDone,
                        GCost = g,
                        HCost = CalculateHeuristic(allTasks, newTasksDone, user),
                        Parent = currentNode
                    };
                    successor.CalculateFCost();

                    // 4) Verificăm dacă e deja în closedList cu un cost mai bun
                    if (closedList.Contains(successor))
                        continue;

                    // Verificăm dacă există deja un nod identic în openList
                    var existing = openList.FirstOrDefault(n => n.Equals(successor));
                    if (existing == null)
                    {
                        // Nu există => îl adăugăm
                        openList.Add(successor);
                    }
                    else
                    {
                        // Există, dar dacă successor are un GCost mai mic, îl "upgrade"
                        if (successor.GCost < existing.GCost)
                        {
                            existing.GCost = successor.GCost;
                            existing.Parent = currentNode;
                            existing.CalculateFCost();
                        }
                    }
                }
            }

            // Dacă openList s-a golit și n-am întors nimic, înseamnă că nu avem drum complet 
            // (improbabil aici). Returnăm o listă goală.
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
