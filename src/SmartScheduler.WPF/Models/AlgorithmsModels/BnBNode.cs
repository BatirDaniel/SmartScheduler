using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Models.AlgorithmsModels
{
    public class BnBNode
    {
        public int Index { get; set; }              // indexul sarcinii curente
        public double CurrentHoursSum { get; set; } // total ore folosite
        public int CurrentPrioritySum { get; set; } // suma priorităților
        public List<TaskModel> SelectedTasks { get; set; } = new List<TaskModel>();
    }
}
