using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        // Parola va fi stocată sub formă de hash (folosind BCrypt).
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Exemplu simplu de stocare a timpului liber al utilizatorului (în ore/zi).
        // În funcție de nevoi, îl poți extinde la intervale de timp, zile ale săptămânii etc.
        public double FreeHoursPerDay { get; set; }

        // Relație One-to-Many cu lista de intervale de timp liber.
        public List<FreeTimeInterval>? FreeTimeIntervals { get; set; }

        // Relație 1 - n (un user are mai multe HobbyModel)
        public List<HobbyModel>? Hobbies { get; set; }

        // Relație cu TaskModel: un utilizator poate avea mai multe sarcini
        public List<TaskModel>? Tasks { get; set; }
    }
}
