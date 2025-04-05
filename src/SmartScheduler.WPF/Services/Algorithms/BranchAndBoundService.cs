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
        public List<TaskModel> FindBestTaskCombination(List<TaskModel> tasks, double maxHours, User user)
        {
            // Sortăm sarcinile după "EffectivePriority" descrescător, 
            // ca să fie B&B mai eficient
            tasks.Sort((a, b) =>
                 GetPriorityWithHobbyBonus(b, user).CompareTo(GetPriorityWithHobbyBonus(a, user))
            );

            // Pentru Branch and Bound, vom stoca (într-o stivă / coadă) noduri ce conțin:
            // - indexul următoarei sarcini
            // - suma prioritatilor curente
            // - orele folosite până acum
            // - sarcinile selectate până acum
            Stack<BnBNode> stack = new Stack<BnBNode>();

            BnBNode bestSolution = new BnBNode
            {
                CurrentPrioritySum = 0,
                CurrentHoursSum = 0,
                Index = 0,
                SelectedTasks = new List<TaskModel>()
            };
            BnBNode bestFound = bestSolution;
            stack.Push(bestSolution);

            while (stack.Count > 0)
            {
                var currentNode = stack.Pop();
                if (currentNode.Index >= tasks.Count)
                {
                    if (currentNode.CurrentPrioritySum > bestFound.CurrentPrioritySum)
                    {
                        bestFound = currentNode;
                    }
                    continue;
                }

                var task = tasks[currentNode.Index];
                double newHours = currentNode.CurrentHoursSum + task.RequiredHours;
                int effectivePriority = GetPriorityWithHobbyBonus(task, user);

                // Ramură 1: includem taskul, dacă nu depășim orele
                if (newHours <= maxHours)
                {
                    var withTask = new BnBNode
                    {
                        Index = currentNode.Index + 1,
                        CurrentHoursSum = newHours,
                        CurrentPrioritySum = currentNode.CurrentPrioritySum + effectivePriority,
                        SelectedTasks = new List<TaskModel>(currentNode.SelectedTasks)
                    };
                    withTask.SelectedTasks.Add(task);
                    stack.Push(withTask);
                }

                // Ramură 2: excludem taskul
                var withoutTask = new BnBNode
                {
                    Index = currentNode.Index + 1,
                    CurrentHoursSum = currentNode.CurrentHoursSum,
                    CurrentPrioritySum = currentNode.CurrentPrioritySum,
                    SelectedTasks = new List<TaskModel>(currentNode.SelectedTasks)
                };
                stack.Push(withoutTask);

                // Updatăm bestFound
                if (currentNode.CurrentPrioritySum > bestFound.CurrentPrioritySum)
                {
                    bestFound = currentNode;
                }
            }

            return bestFound.SelectedTasks;
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
