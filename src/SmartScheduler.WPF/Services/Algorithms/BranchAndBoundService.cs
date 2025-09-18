using SmartScheduler.WPF.Models;
using SmartScheduler.WPF.Models.AlgorithmsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Services.Algorithms
{
    public class BranchAndBoundService
    {
        private static BranchAndBoundService? _instance;

        private BranchAndBoundService() { }

        public static BranchAndBoundService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new BranchAndBoundService();
            }
            return _instance;
        }

        /// <summary>
        ///  Exemplu de rezolvare a problemei "selectează sarcini" 
        ///  astfel încât să maximizăm suma priorităților fără a depăși "maxHours" (timp limitat).
        ///  Returnează lista de TaskModel selectate.
        /// </summary>
        /// <param name="tasks">Lista completă de sarcini.</param>
        /// <param name="maxHours">Numărul maxim de ore disponibile.</param>
        /// <returns>Lista de sarcini alese de BnB pentru maximizarea priorității totale.</returns>
        public List<TaskModel> FindBestTaskCombination(List<TaskModel> tasks,double maxHours,User user)
        {
            // 1️⃣ – sortăm întâi după prioritate efectivă (ca şi până acum)
            tasks = tasks
                .OrderByDescending(t => GetPriorityWithHobbyBonus(t, user))
                .ToList();

            var rng = new Random();             // generator unic pe metodă
            var stack = new Stack<BnBNode>();
            var bestScore = 0;
            var bestSets = new List<List<TaskModel>>();

            stack.Push(new BnBNode
            {
                Index = 0,
                CurrentHoursSum = 0,
                CurrentPrioritySum = 0,
                SelectedTasks = new()
            });

            while (stack.Count > 0)
            {
                var node = stack.Pop();

                // 📈 actualizăm colecţia de soluţii optime
                if (node.CurrentPrioritySum > bestScore)
                {
                    bestScore = node.CurrentPrioritySum;
                    bestSets = new() { node.SelectedTasks };
                }
                else if (node.CurrentPrioritySum == bestScore)
                {
                    bestSets.Add(node.SelectedTasks);
                }

                // am parcurs toată lista → nimic de extins
                if (node.Index >= tasks.Count) continue;

                var task = tasks[node.Index];
                var withBonus = GetPriorityWithHobbyBonus(task, user);
                var hoursWithTask = node.CurrentHoursSum + task.RequiredHours;

                // ➕ ramura „ia task‑ul”
                if (hoursWithTask <= maxHours)
                {
                    stack.Push(new BnBNode
                    {
                        Index = node.Index + 1,
                        CurrentHoursSum = hoursWithTask,
                        CurrentPrioritySum = node.CurrentPrioritySum + withBonus,
                        SelectedTasks = new List<TaskModel>(node.SelectedTasks) { task }
                    });
                }

                // ➖ ramura „sari peste task”
                stack.Push(new BnBNode
                {
                    Index = node.Index + 1,
                    CurrentHoursSum = node.CurrentHoursSum,
                    CurrentPrioritySum = node.CurrentPrioritySum,
                    SelectedTasks = new List<TaskModel>(node.SelectedTasks)
                });
            }

            // 2️⃣ – alegem la întâmplare una dintre cele optime
            return bestSets[rng.Next(bestSets.Count)];
        }

        private int GetPriorityWithHobbyBonus(TaskModel task, User user)
        {
            int basePriority = (int)task.Priority;

            // Bonus de +1 dacă userul are un hobby ce match-uiește task.Category
            if (user.Hobbies != null && user.Hobbies.Any(h => h.HobbyName == task.Category))
            {
                basePriority += 1;
            }

            return basePriority;
        }
    }
}
