using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Models
{
    [Table("Hobbies")]
    public class HobbyModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HobbyName { get; set; } = string.Empty;

        // Legătura cu User
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
