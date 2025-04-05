using Microsoft.EntityFrameworkCore;
using SmartScheduler.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Repository.Implementations
{
    public class SmartSchedulerRepository : ISmartSchedulerRepository
    {

        SmartSchedulerContext _context;

        public TaskModel AddTask(TaskModel task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return task;
        }

        public TaskModel? GetTaskById(int taskId)
        {
            return _context.Tasks
                          .Include(t => t.User)
                          .FirstOrDefault(t => t.Id == taskId);
        }

        public List<TaskModel> GetAllTasks()
        {
            return _context.Tasks
                          .Include(t => t.User)
                          .ToList();
        }

        public TaskModel UpdateTask(TaskModel task)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
            return task;
        }

        public bool DeleteTask(int taskId)
        {
            var taskToDelete = _context.Tasks.Find(taskId);
            if (taskToDelete == null) return false;

            _context.Tasks.Remove(taskToDelete);
            _context.SaveChanges();
            return true;
        }

        // FREE TIME INTERVALS
        public FreeTimeInterval AddFreeTimeInterval(FreeTimeInterval interval)
        {
            _context.FreeTimeIntervals.Add(interval);
            _context.SaveChanges();
            return interval;
        }

        public List<FreeTimeInterval> GetFreeTimeIntervalsByUserId(int userId)
        {
            return _context.FreeTimeIntervals
                          .Where(f => f.UserId == userId)
                          .ToList();
        }

        public bool DeleteFreeTimeInterval(int intervalId)
        {
            var interval = _context.FreeTimeIntervals.Find(intervalId);
            if (interval == null) return false;

            _context.FreeTimeIntervals.Remove(interval);
            _context.SaveChanges();
            return true;
        }
    }

}

