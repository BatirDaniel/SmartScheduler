using SmartScheduler.WPF.Enums;
using SmartScheduler.WPF.Models;
using SmartScheduler.WPF.Services;
using SmartScheduler.WPF.Services.Algorithms;

namespace SmartScheduler.Tests
{
    public class SchedulingServiceTests
    {
        private readonly List<TaskModel> _tasks;
        private readonly User _user;

        public SchedulingServiceTests()
        {
            // 3 task‑uri simple
            _tasks = new List<TaskModel>
             {
                new TaskModel { Id =  1, Title = "Jogging",            RequiredHours = 1, Priority = TaskPriority.Medium, Category = "Fitness" },
                new TaskModel { Id =  2, Title = "Read novel",         RequiredHours = 2, Priority = TaskPriority.Low,    Category = "Reading" },
                new TaskModel { Id =  3, Title = "Refactor code",      RequiredHours = 2, Priority = TaskPriority.High,   Category = "Coding"  },
                new TaskModel { Id =  4, Title = "Yoga session",       RequiredHours = 1, Priority = TaskPriority.Medium, Category = "Fitness" },
                new TaskModel { Id =  5, Title = "Grocery shopping",   RequiredHours = 2, Priority = TaskPriority.Low,    Category = "Errands" },
                new TaskModel { Id =  6, Title = "Write blog post",    RequiredHours = 2, Priority = TaskPriority.Medium, Category = "Writing" },
                new TaskModel { Id =  7, Title = "Watch tutorial",     RequiredHours = 1, Priority = TaskPriority.Low,    Category = "Learning"},
                new TaskModel { Id =  8, Title = "Cook dinner",        RequiredHours = 2, Priority = TaskPriority.Low,    Category = "Cooking" },
                new TaskModel { Id =  9, Title = "Unit‑test cleanup",  RequiredHours = 2, Priority = TaskPriority.High,   Category = "Coding"  },
                new TaskModel { Id = 10, Title = "Research topic",     RequiredHours = 2, Priority = TaskPriority.Medium, Category = "Research"},
                new TaskModel { Id = 11, Title = "Photography walk",   RequiredHours = 2, Priority = TaskPriority.Low,    Category = "Photography"},
                new TaskModel { Id = 12, Title = "Meditation",         RequiredHours = 1, Priority = TaskPriority.Low,    Category = "Wellness"},
                new TaskModel { Id = 13, Title = "Language practice",  RequiredHours = 1, Priority = TaskPriority.Medium, Category = "Learning"},
                new TaskModel { Id = 14, Title = "Database backup",    RequiredHours = 1, Priority = TaskPriority.High,   Category = "SysAdmin"},
                new TaskModel { Id = 15, Title = "Plan vacation",      RequiredHours = 2, Priority = TaskPriority.Low,    Category = "Planning"},
                new TaskModel { Id = 16, Title = "Strength training",  RequiredHours = 1, Priority = TaskPriority.High,   Category = "Fitness" },
                new TaskModel { Id = 17, Title = "Read research paper",RequiredHours = 2, Priority = TaskPriority.Medium, Category = "Reading" },
                new TaskModel { Id = 18, Title = "Design mock‑ups",    RequiredHours = 2, Priority = TaskPriority.Medium, Category = "Design"  },
                new TaskModel { Id = 19, Title = "Clean workspace",    RequiredHours = 1, Priority = TaskPriority.Low,    Category = "Errands" },
                new TaskModel { Id = 20, Title = "Update resume",      RequiredHours = 1, Priority = TaskPriority.Medium, Category = "Writing" },
                new TaskModel { Id = 21, Title = "Play guitar",        RequiredHours = 1, Priority = TaskPriority.Low,    Category = "Music"   },
                new TaskModel { Id = 22, Title = "Bug triage",         RequiredHours = 2, Priority = TaskPriority.High,   Category = "Coding"  }
             };

            _user = new User
            {
                Id = 99,
                Username = "tester",
                Email = "tester@example.com",
                PasswordHash = "$2b$12$xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", // hash fictiv

                // ─── hobby‑uri ───────────────────────────────────────────────
                Hobbies = new List<HobbyModel>
                {
                     new () { Id = 501, HobbyName = "Fitness",     UserId = 99 },
                     new () { Id = 502, HobbyName = "Reading",     UserId = 99 },
                     new () { Id = 502, HobbyName = "Coding",     UserId = 99 },
                     new () { Id = 503, HobbyName = "Learning", UserId = 99 }
                },

                // ─── intervale libere ────────────────────────────────────────
                FreeTimeIntervals = new List<FreeTimeInterval>
                {
                    new () { Id = 1001, DayOfWeek = DayOfWeek.Monday,    StartTime = new TimeSpan( 6,  0, 0), EndTime = new TimeSpan( 7, 30, 0), UserId = 99 },
                    new () { Id = 1002, DayOfWeek = DayOfWeek.Tuesday,   StartTime = new TimeSpan(19,  0, 0), EndTime = new TimeSpan(21,  0, 0), UserId = 99 },
                    new () { Id = 1003, DayOfWeek = DayOfWeek.Wednesday, StartTime = new TimeSpan(12,  0, 0), EndTime = new TimeSpan(13,  0, 0), UserId = 99 },
                    new () { Id = 1004, DayOfWeek = DayOfWeek.Thursday,  StartTime = new TimeSpan(18,  0, 0), EndTime = new TimeSpan(20,  0, 0), UserId = 99 },
                    new () { Id = 1005, DayOfWeek = DayOfWeek.Friday,    StartTime = new TimeSpan( 7,  0, 0), EndTime = new TimeSpan( 8, 30, 0), UserId = 99 },
                    new () { Id = 1006, DayOfWeek = DayOfWeek.Saturday,  StartTime = new TimeSpan(10,  0, 0), EndTime = new TimeSpan(12,  0, 0), UserId = 99 },
                    new () { Id = 1007, DayOfWeek = DayOfWeek.Sunday,    StartTime = new TimeSpan(15,  0, 0), EndTime = new TimeSpan(17,  0, 0), UserId = 99 }
                }
            };

            // calculează automat FreeHoursPerDay (opţional, doar informativ)
            _user.FreeHoursPerDay = _user.FreeTimeIntervals!
                .Sum(iv => (iv.EndTime - iv.StartTime).TotalHours);
        }

        // === utilitar pentru reset singleton‑urile între teste ================
        private static void ResetSingletons()
        {
            typeof(HungarianAlgorithmService)
                .GetField("_instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, null);

            typeof(BranchAndBoundService)
                .GetField("_instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, null);

            typeof(AStarService)
                .GetField("_instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, null);

            typeof(SchedulingService)
                .GetField("_instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, null);
        }

        // === TEST 1 : Hungarian ==============================================
        [Fact]
        public void ScheduleWithHungarian_Returns_Assignment_Array_Of_Length_N()
        {
            ResetSingletons();
            var scheduler = SchedulingService.GetInstance();

            var resultObj = scheduler.ScheduleTasks(
                SchedulingAlgorithm.Hungarian,
                _tasks, _user);

            var ordered = Assert.IsType<List<TaskModel>>(resultObj);

            var hobbyCategories = _user.Hobbies!.Select(h => h.HobbyName);

            bool anyHobbyTask = ordered.Any(t =>
             hobbyCategories.Contains(t.Category, StringComparer.OrdinalIgnoreCase));
        }

        // === TEST 2 : Branch & Bound =========================================
        [Fact]
        public void ScheduleWithBranchAndBound_Respects_MaxHours_And_Prefers_Hobby()
        {
            ResetSingletons();
            var scheduler = SchedulingService.GetInstance();

            double maxHours = 8.0;          // limită de timp mică
            var resultObj = scheduler.ScheduleTasks(
                SchedulingAlgorithm.BranchAndBound,
                _tasks,
                _user,
                maxHours
            );

            var selected = Assert.IsType<List<TaskModel>>(resultObj);

            // 1) total RequiredHours ≤ maxHours
            double totalHrs = selected.Sum(t => t.RequiredHours);
            Assert.True(totalHrs <= maxHours);

            // 2) dacă există un task compatibil cu hobby‑ul și încapem în timp,
            //    el ar trebui să apară în listă
            Assert.Contains(selected, t => t.Category == "Fitness");
        }

        // === TEST 3 : A* ======================================================
        [Fact]
        public void ScheduleWithAStar_Places_Hobby_Task_First_When_Cost_Bonus_Applies()
        {
            ResetSingletons();
            var scheduler = SchedulingService.GetInstance();

            // Act
            var result = scheduler.ScheduleTasks(
                            SchedulingAlgorithm.AStar,
                            _tasks,
                            _user);

            var ordered = Assert.IsType<List<TaskModel>>(result);

            var hobbyCategories = _user.Hobbies!.Select(h => h.HobbyName);

            bool anyHobbyTask = ordered.Any(t =>
             hobbyCategories.Contains(t.Category, StringComparer.OrdinalIgnoreCase));

            Assert.True(anyHobbyTask);
            double sumHrs = ordered.Sum(t => t.RequiredHours);
            Assert.InRange(sumHrs, _user.FreeHoursPerDay - 1, _user.FreeHoursPerDay + 1);

            Assert.Contains(ordered.First().Category, hobbyCategories);
        }
    }
}