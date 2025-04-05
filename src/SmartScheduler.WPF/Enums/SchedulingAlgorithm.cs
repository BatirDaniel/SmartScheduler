using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Enums
{

    /// <summary>
    ///  Enum care specifică ce algoritm dorim să folosim pentru planificare
    /// </summary>
    public enum SchedulingAlgorithm
    {
        Hungarian,
        BranchAndBound,
        AStar
    }
}
