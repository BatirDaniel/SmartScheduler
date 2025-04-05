using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Models.AlgorithmsModels
{
    public class AStarNode
    {
        public List<TaskModel> TasksDone { get; set; } = new List<TaskModel>();
        public double GCost { get; set; }  // cost parcurs
        public double HCost { get; set; }  // euristică
        public double FCost { get; set; }  // G + H
        public AStarNode? Parent { get; set; }

        public void CalculateFCost()
        {
            FCost = GCost + HCost;
        }
    }
}
