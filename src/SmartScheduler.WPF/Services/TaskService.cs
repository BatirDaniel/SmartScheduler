using SmartScheduler.WPF.Models;
using SmartScheduler.WPF.Repository;
using SmartScheduler.WPF.Repository.Implementations;
using SmartScheduler.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartScheduler.WPF.Services
{
    public class TaskService
    {
        private static TaskService? _instance;
        private readonly ISmartSchedulerRepository _smartSchedulerRepository;

        private TaskService()
        {
            _smartSchedulerRepository = new SmartSchedulerRepository();
        }

        public static TaskService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TaskService();
            }
            return _instance;
        }

        public ObservableCollection<TaskItemViewModel> GetTasks(DateTime? selectedDate, int userId)
        {
            DateTime date = selectedDate?.Date ?? DateTime.UtcNow.Date;
            List<TaskModel> tasks = _smartSchedulerRepository.GetAllTasksByUserAndDay(userId, date);

            List<TaskItemViewModel> result = tasks
                .Where(x => x.DueDate.HasValue && x.DueDate.Value.Date == date)
                .OrderBy(x => x.TaskOrder)
                .Select(x => new TaskItemViewModel
                {
                    Title = x.Title,
                    Time = x.DueDate.Value.ToString("hh:mm"),
                    Color = TaskPriorityHelper.GetColorForPriority(x.Priority)
                })
                .ToList();

            if (result == null || !result.Any())
                return new ObservableCollection<TaskItemViewModel>(new List<TaskItemViewModel> { new TaskItemViewModel { Title = "There are no tasks available today!" } });

            return new ObservableCollection<TaskItemViewModel>(result);
        }

        public double GetPlannedTime(int userId, DateTime date, List<TaskModel> tasks)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            if (tasks == null || !tasks.Any())
                tasks = _smartSchedulerRepository.GetAllTasksByUserAndDay(userId, date);

            return tasks
                .Where(t => t.DueDate >= startOfDay && t.DueDate < endOfDay)
                .Sum(t => t.RequiredHours);
        }

        public double GetFreeTime(int userId, DateTime date, List<TaskModel> tasks, double maxAvailableHoursPerDay = 8)
        {
            var plannedTime = GetPlannedTime(userId, date, tasks);
            return Math.Max(0, maxAvailableHoursPerDay - plannedTime);
        }

        public double GetEfficiency(int userId, DateTime date, List<TaskModel> tasks, double maxAvailableHoursPerDay = 8)
        {
            var plannedTime = GetPlannedTime(userId, date, tasks);
            return maxAvailableHoursPerDay > 0
                ? Math.Round((plannedTime / maxAvailableHoursPerDay) * 100, 2)
                : 0;
        }
    }
}