using SmartScheduler.WPF.Enums;

namespace SmartScheduler.WPF
{
    public static class TaskPriorityHelper
    {
        public static string GetColorForPriority(TaskPriority priority)
        {
            switch (priority)
            {
                case TaskPriority.Low:
                    return "Green";  // Low priority - Green
                case TaskPriority.Medium:
                    return "Orange"; // Medium priority - Orange
                case TaskPriority.High:
                    return "Red";    // High priority - Red
                default:
                    return "Gray";   // Default color
            }
        }
    }
}