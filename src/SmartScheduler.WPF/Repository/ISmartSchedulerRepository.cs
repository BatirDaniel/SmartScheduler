using SmartScheduler.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Repository
{
    public interface ISmartSchedulerRepository
    {
        TaskModel AddTask(TaskModel task);
        TaskModel? GetTaskById(int taskId);
        List<TaskModel> GetAllTasks();
        TaskModel UpdateTask(TaskModel task);
        bool DeleteTask(int taskId);

        // FREE TIME INTERVALS
        FreeTimeInterval AddFreeTimeInterval(FreeTimeInterval interval);
        List<FreeTimeInterval> GetFreeTimeIntervalsByUserId(int userId);
        bool DeleteFreeTimeInterval(int intervalId);
    }
}
