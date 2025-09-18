using Microsoft.EntityFrameworkCore;
using SmartScheduler.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Repository.Implementations
{
    public class TaskRepository : ITaskRepository
    {
        SmartSchedulerContext _context;

        public TaskRepository()
        {
            _context = SmartSchedulerContext.GetInstance();
        }

        public TaskModel AddTask(TaskModel task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return task;
        }

        public void AddOrUpdateTasks(User user, IEnumerable<TaskModel> tasks, DateTime selectedDate)
        {
            foreach (var incoming in tasks)
            {
                incoming.UserId = user.Id;
                incoming.DueDate = selectedDate;

                // === INSERT NOU =======================================
                if (incoming.Id == 0)
                {
                    _context.Tasks.Add(incoming);
                    continue;
                }

                // === EXISTĂ? CĂUTĂM ÎN CONTEXT/DATABASE ===============
                var dbEntity = _context.Tasks                // ① caut în ChangeTracker
                                       .FirstOrDefault(t => t.Id == incoming.Id);

                if (dbEntity == null)
                {
                    dbEntity = _context.Tasks                // ② caut direct în DB
                                         .AsNoTracking()
                                         .FirstOrDefault(t => t.Id == incoming.Id);
                }

                if (dbEntity == null)
                {
                    // ► rândul a fost şters între timp – inserăm ca nou
                    incoming.Id = 0;
                    _context.Tasks.Add(incoming);
                }
                else
                {
                    // ► UPDATE: sincronizăm valorile în entitatea deja urmărită
                    _context.Entry(dbEntity).CurrentValues.SetValues(incoming);
                    // RowVersion (dacă există) rămâne cea din dbEntity => OK
                }
            }

            _context.SaveChanges();                         // un singur hit DB
        }
    }
}
