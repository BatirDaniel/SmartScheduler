using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Models
{
    [Table("FreeTimeIntervals")]
    public class FreeTimeInterval
    {
        [Key]
        public int Id { get; set; }

        // Ziua săptămânii. Poți folosi DayOfWeek (enum) sau un int/DateTime, în funcție de nevoi.
        // Exemplu: (int)DayOfWeek.Monday = 1, etc.
        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        // Timpul de start al intervalului (ex. 08:00)
        [Required]
        public TimeSpan StartTime { get; set; }

        // Timpul de sfârșit al intervalului (ex. 12:00)
        [Required]
        public TimeSpan EndTime { get; set; }

        // Cheie străină spre utilizator
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
