using Microsoft.EntityFrameworkCore;
using SmartScheduler.WPF.Models;


namespace SmartScheduler.WPF.Repository
{
    public class SmartSchedulerContext : DbContext
    {
        private static SmartSchedulerContext? _instance;
        public SmartSchedulerContext() : base() {
        }

        public SmartSchedulerContext(DbContextOptions<SmartSchedulerContext> options)
            : base(options) {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<FreeTimeInterval> FreeTimeIntervals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=SmartScheduler.db");
            }
        }

        public static SmartSchedulerContext GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SmartSchedulerContext();
            }
            return _instance;
        }
    }
}
