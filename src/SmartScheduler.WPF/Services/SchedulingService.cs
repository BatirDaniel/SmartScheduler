using SmartScheduler.WPF.Enums;
using SmartScheduler.WPF.Models;
using SmartScheduler.WPF.Services.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartScheduler.WPF.Services
{
    public class SchedulingService
    {
        private static SchedulingService? _instance;

        private SchedulingService()
        { }

        public static SchedulingService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SchedulingService();
            }
            return _instance;
        }

        /// <summary>
        ///  Metodă principală care, în funcție de algoritmul cerut, apelează logica respectivă.
        ///  Returnează o listă de TaskModel în ordinea/structura "planificată"
        ///  (sau un assignment vector, în cazul Hungarian).
        ///
        ///  - tasks: lista de sarcini pe care vrem să le planificăm
        ///  - user: utilizatorul (pentru hobby, preferințe, etc.)
        ///  - maxHours: opțional, dacă vrem să limităm durata totală (folosit mai ales la Branch & Bound)
        /// </summary>
        public List<TaskModel> ScheduleTasks(SchedulingAlgorithm algorithm, List<TaskModel> tasks, User user, double? maxHours = null)
        {
            var defaultTasks = TaskSeeder.GetCsvTasks();
            switch (algorithm)
            {
                case SchedulingAlgorithm.Hungarian:
                    return ScheduleWithHungarian(defaultTasks, user);

                case SchedulingAlgorithm.BranchAndBound:
                    return ScheduleWithBranchAndBound(defaultTasks, user, maxHours ?? 6);

                case SchedulingAlgorithm.AStar:
                    return ScheduleWithAStar(defaultTasks, user);

                default:
                    throw new NotImplementedException("Algoritm de planificare necunoscut.");
            }
        }

        /// <summary>
        ///  1) Hungarian:
        ///     - generează o matrice de cost pentru (task vs. slot),
        ///       folosind HungarianAlgorithmService,
        ///       scade costul pentru hobby-urile userului.
        ///     - returnează vectorul "assignment[i] = coloana".
        /// </summary>
        private List<TaskModel> ScheduleWithHungarian(List<TaskModel> tasks, User user)
        {
            var hungarian = HungarianAlgorithmService.GetInstance();
            var ordered = hungarian.GetTaskOrderWithHobbyBonus(tasks, user);

            // extrage prefixul exact = FreeHoursPerDay
            return ordered.Take(7).ToList();
        }

        /// <summary>
        ///  2) Branch and Bound:
        ///     - maximizăm prioritatea ( + bonus pentru hobby ) în limita de ore (maxHours).
        ///     - returnează lista de sarcini "selectate".
        /// </summary>
        private List<TaskModel> ScheduleWithBranchAndBound(List<TaskModel> tasks, User user, double maxHours)
        {
            var bnb = BranchAndBoundService.GetInstance();
            // presupunem că avem o metodă "FindBestTaskCombination" care ia user ca parametru
            // și oferă un "bonus" la prioritate dacă task.Category == un hobby al userului.
            List<TaskModel> bestCombo = bnb.FindBestTaskCombination(tasks, maxHours, user);
            return bestCombo;
        }

        /// <summary>
        ///  3) A*:
        ///     - caută "ordinea optimă" de parcurgere a sarcinilor,
        ///       cost = RequiredHours - bonusHobby.
        /// </summary>
        private List<TaskModel> ScheduleWithAStar(List<TaskModel> tasks, User user)
        {
            var aStar = AStarService.GetInstance();
            var ordered = aStar.FindOptimalTaskOrderWithHobby(tasks, user);

            return ordered.Take(7).ToList();
        }

    }
}