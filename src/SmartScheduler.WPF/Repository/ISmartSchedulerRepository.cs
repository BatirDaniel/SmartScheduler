using SmartScheduler.WPF.Models;
using System;
using System.Collections.Generic;

namespace SmartScheduler.WPF.Repository
{
    public interface ISmartSchedulerRepository
    {
        TaskModel AddTask(TaskModel task);

        TaskModel? GetTaskById(int taskId);

        List<TaskModel> GetAllTasksByUserAndDay(int userId, DateTime? date);

        TaskModel UpdateTask(TaskModel task);

        bool DeleteTask(int taskId);

        // FREE TIME INTERVALS
        FreeTimeInterval AddFreeTimeInterval(FreeTimeInterval interval);

        List<FreeTimeInterval> GetFreeTimeIntervalsByUserId(int userId);

        bool DeleteFreeTimeInterval(int intervalId);
    }
}