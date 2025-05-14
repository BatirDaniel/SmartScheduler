using SmartScheduler.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Repository
{
    public interface ITaskRepository
    {
        TaskModel AddTask(TaskModel task);
        void AddOrUpdateTasks(User user, IEnumerable<TaskModel> tasks, DateTime selectedDate);
    }
}
