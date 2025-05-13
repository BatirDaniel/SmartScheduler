using SmartScheduler.WPF.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartScheduler.WPF.Models
{
    [Table("Tasks")]
    public class TaskModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public DateTime? PlannedDate { get; set; }

        [Required]
        public TaskStatus Status { get; set; } = TaskStatus.Open;

        public string? Description { get; set; }

        [Required]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        // Durata necesară în ore
        public double RequiredHours { get; set; }

        // Data limită (deadline)
        public DateTime? DueDate { get; set; }

        // Tag / categorie a sarcinii (ex. "Fitness", "Reading", "Coding")
        public string? Category { get; set; }

        // Legătura cu UserModel (FK)
        public int UserId { get; set; }

        public User? User { get; set; }
    }
}