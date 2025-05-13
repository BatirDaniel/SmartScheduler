using SmartScheduler.WPF.Enums;
using SmartScheduler.WPF.Models;
using SmartScheduler.WPF.Repository.Implementations;
using SmartScheduler.WPF.Services;
using SmartScheduler.WPF.Session;
using SmartScheduler.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmartScheduler.WPF.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : Page, INotifyPropertyChanged
    {
        private readonly TaskService _taskService = TaskService.GetInstance();

        private readonly SchedulingService _schedulingService = SchedulingService.GetInstance();

        private readonly SmartSchedulerRepository _smartSchedulerRepository = new SmartSchedulerRepository();

        private ObservableCollection<TaskItemViewModel> _tasks;

        private string _plannedTime;

        private string _freeTime;

        private string _efficiency;

        private string _quickStats;

        public string PlannedTime
        {
            get => _plannedTime;
            set
            {
                _plannedTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlannedTime)));
            }
        }

        public string FreeTime
        {
            get => _freeTime;
            set
            {
                _freeTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FreeTime)));
            }
        }

        public string Efficiency
        {
            get => _efficiency;
            set
            {
                _efficiency = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Efficiency)));
            }
        }

        public string QuickStats
        {
            get => _quickStats;
            set
            {
                _quickStats = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QuickStats)));
            }
        }

        public ObservableCollection<TaskItemViewModel> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tasks)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DashboardView()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainCalendar.SelectedDate = DateTime.Today;
            MainCalendar.DisplayDate = DateTime.Today;

            RefreshTasks();
        }

        private void MainCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshTasks();
            Keyboard.ClearFocus();
        }

        private void RefreshTasks(bool isPlanning = false)
        {
            var userId = SessionManager.LoggedInUser.Id;
            var date = MainCalendar.SelectedDate.GetValueOrDefault();
            var tasks = _smartSchedulerRepository.GetAllTasksByUserAndDay(userId, date);

            if (!isPlanning && CurrentDate != null && MainCalendar.SelectedDate.HasValue)
            {
                CurrentDate.Text = date.ToString("dddd, MMMM dd");

                List<TaskItemViewModel> resultTasks = tasks
                    .Select(x => new TaskItemViewModel
                    {
                        Title = x.Title,
                        Description = x.Description,
                        Time = x.DueDate.Value.ToString("hh:mm"),
                        Color = TaskPriorityHelper.GetColorForPriority(x.Priority)
                    })
                    .ToList();

                Tasks = new ObservableCollection<TaskItemViewModel>(resultTasks);
            }
            else
            {
                User? user = SessionManager.LoggedInUser;
                tasks = _schedulingService.ScheduleTasks(SchedulingAlgorithm.AStar, tasks, user);

                List<TaskItemViewModel> plannedTasks = tasks
                    .Select(x => new TaskItemViewModel
                    {
                        Title = x.Title,
                        Description = x.Description,
                        Time = x.DueDate.GetValueOrDefault().ToString("hh:mm"),
                        Color = TaskPriorityHelper.GetColorForPriority(x.Priority)
                    })
                    .ToList();

                Tasks = new ObservableCollection<TaskItemViewModel>(plannedTasks);
            }

            var planned = _taskService.GetPlannedTime(userId, date, tasks);
            var free = _taskService.GetFreeTime(userId, date, tasks);
            var efficiency = _taskService.GetEfficiency(userId, date, tasks);

            int plannedHours = (int)planned;
            int plannedMinutes = (int)((planned - plannedHours) * 60);

            int freeHours = (int)free;
            int freeMinutes = (int)((free - freeHours) * 60);

            PlannedTime = $"{plannedHours} h {plannedMinutes} m";
            FreeTime = $"{freeHours} h {freeMinutes} m";
            Efficiency = $"{efficiency:F1}%";

            var minTask = tasks.OrderBy(x => x.RequiredHours).FirstOrDefault();
            QuickStats = $"➤ {minTask?.Description}" ?? "No task available";
        }

        private void planTasksBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshTasks(true);
        }
    }
}