using SmartScheduler.WPF.Enums;
using SmartScheduler.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = SmartScheduler.WPF.Enums.TaskStatus;

namespace SmartScheduler.WPF.Services
{
    public static class TaskSeeder
    {
        public static List<TaskModel> GetCsvTasks() => new()
        {
new TaskModel
{
    Id = 1,
    Title = "Complete project report",
    Description = "Finalize the project report for the client and send it via email.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 3.0,
    DueDate = new DateTime(2025, 5, 14, 12, 0, 0),
    Category = "Work"
},

new TaskModel
{
    Id = 2,
    Title = "Team meeting: Design review",
    Description = "Discuss the progress of the homepage design with the team.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.High,
    RequiredHours = 2.0,
    DueDate = new DateTime(2025, 5, 14, 12, 30, 0),
    Category = "Work"
},

new TaskModel
{
    Id = 3,
    Title = "Write blog post: New trends in technology",
    Description = "Research and write a blog post about the latest trends in technology.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 4.0,
    DueDate = new DateTime(2025, 5, 14, 17, 0, 0),
    Category = "Writing"
},

new TaskModel
{
    Id = 4,
    Title = "Read book on marketing strategies",
    Description = "Read a chapter of the book “Marketing Strategies for Startups”.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Low,
    RequiredHours = 1.5,
    DueDate = new DateTime(2025, 5, 14, 16, 0, 0),
    Category = "Learning"
},

new TaskModel
{
    Id = 5,
    Title = "Code new feature for app",
    Description = "Develop the new feature for the mobile app as discussed in the meeting.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.High,
    RequiredHours = 3.0,
    DueDate = new DateTime(2025, 5, 14, 19, 0, 0),
    Category = "Development"
},

new TaskModel
{
    Id = 6,
    Title = "Client call: Project update",
    Description = "Call the client to provide an update on the project status and discuss next steps.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 1.0,
    DueDate = new DateTime(2025, 5, 15, 10, 0, 0),
    Category = "Work"
},

new TaskModel
{
    Id = 7,
    Title = "Attend webinar on data analytics",
    Description = "Join the webinar on the latest data analytics tools and techniques.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Low,
    RequiredHours = 2.0,
    DueDate = new DateTime(2025, 5, 15, 13, 0, 0),
    Category = "Learning"
},

new TaskModel
{
    Id = 8,
    Title = "Write email to potential partners",
    Description = "Write a proposal email to potential business partners about collaboration opportunities.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 1.0,
    DueDate = new DateTime(2025, 5, 15, 15, 0, 0),
    Category = "Work"
},

new TaskModel
{
    Id = 9,
    Title = "Create weekly progress presentation",
    Description = "Prepare a PowerPoint presentation summarizing the weekly progress for the team.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.High,
    RequiredHours = 2.5,
    DueDate = new DateTime(2025, 5, 15, 18, 0, 0),
    Category = "Work"
},

new TaskModel
{
    Id = 10,
    Title = "Morning exercise: Yoga",
    Description = "Perform a 30-minute yoga session for relaxation and flexibility.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Low,
    RequiredHours = 0.5,
    DueDate = new DateTime(2025, 5, 16, 7, 30, 0),
    Category = "Fitness"
},

new TaskModel
{
    Id = 11,
    Title = "Update website content",
    Description = "Update the homepage content with new project achievements and services offered.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 2.0,
    DueDate = new DateTime(2025, 5, 16, 11, 0, 0),
    Category = "Development"
},

new TaskModel
{
    Id = 12,
    Title = "Write product description for website",
    Description = "Write a compelling product description for the new product line.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.High,
    RequiredHours = 1.5,
    DueDate = new DateTime(2025, 5, 16, 13, 0, 0),
    Category = "Writing"
},

new TaskModel
{
    Id = 13,
    Title = "Client meeting: New feature demo",
    Description = "Prepare a demonstration for the client to showcase the newly implemented feature.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.High,
    RequiredHours = 2.0,
    DueDate = new DateTime(2025, 5, 16, 16, 0, 0),
    Category = "Work"
},

new TaskModel
{
    Id = 14,
    Title = "Debug application issue",
    Description = "Identify and fix the bug causing the crash in the mobile app.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 3.0,
    DueDate = new DateTime(2025, 5, 16, 19, 0, 0),
    Category = "Development"
},

new TaskModel
{
    Id = 15,
    Title = "Grocery shopping",
    Description = "Buy groceries for the week, including fresh fruits, vegetables, and other essentials.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Low,
    RequiredHours = 1.5,
    DueDate = new DateTime(2025, 5, 16, 19, 30, 0),
    Category = "Personal"
},

new TaskModel
{
    Id = 16,
    Title = "Review performance reports",
    Description = "Go through the monthly performance reports and analyze areas for improvement.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 3.0,
    DueDate = new DateTime(2025, 5, 17, 12, 0, 0),
    Category = "Work"
},

new TaskModel
{
    Id = 17,
    Title = "Update resume with recent projects",
    Description = "Add the recent projects and achievements to your resume for career growth.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Low,
    RequiredHours = 2.0,
    DueDate = new DateTime(2025, 5, 17, 15, 0, 0),
    Category = "Personal"
},

new TaskModel
{
    Id = 18,
    Title = "Attend team building activity",
    Description = "Join the team building activity to improve communication and collaboration within the team.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 2.0,
    DueDate = new DateTime(2025, 5, 17, 17, 30, 0),
    Category = "Work"
},

new TaskModel
{
    Id = 19,
    Title = "Plan next week’s tasks",
    Description = "Review and plan the tasks for the upcoming week, set priorities and deadlines.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Low,
    RequiredHours = 1.0,
    DueDate = new DateTime(2025, 5, 17, 18, 30, 0),
    Category = "Work"
},

new TaskModel
{
    Id = 20,
    Title = "Book doctor’s appointment",
    Description = "Call the doctor’s office to schedule an annual check-up appointment.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Low,
    RequiredHours = 0.5,
    DueDate = new DateTime(2025, 5, 18, 9, 30, 0),
    Category = "Personal"
},

new TaskModel
{
    Id = 21,
    Title = "Prepare for product launch event",
    Description = "Prepare materials, presentations, and agenda for the upcoming product launch event.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.High,
    RequiredHours = 4.0,
    DueDate = new DateTime(2025, 5, 18, 14, 0, 0),
    Category = "Work"
},

new TaskModel
{
    Id = 22,
    Title = "Final review of marketing campaign",
    Description = "Review the final draft of the marketing campaign and make necessary adjustments before launch.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 2.0,
    DueDate = new DateTime(2025, 5, 18, 16, 30, 0),
    Category = "Marketing"
},

new TaskModel
{
    Id = 23,
    Title = "Fix broken links on website",
    Description = "Check for and fix any broken links on the company website.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Low,
    RequiredHours = 1.0,
    DueDate = new DateTime(2025, 5, 18, 17, 0, 0),
    Category = "Development"
},

new TaskModel
{
    Id = 24,
    Title = "Create social media posts for the week",
    Description = "Design and schedule social media posts for the upcoming week.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 3.0,
    DueDate = new DateTime(2025, 5, 19, 12, 0, 0),
    Category = "Marketing"
},

new TaskModel
{
    Id = 25,
    Title = "Exercise: Run 5km",
    Description = "Go for a 5 km run in the morning to stay active and healthy.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Low,
    RequiredHours = 1.0,
    DueDate = new DateTime(2025, 5, 19, 7, 0, 0),
    Category = "Fitness"
},

new TaskModel
{
    Id = 26,
    Title = "Draft project roadmap",
    Description = "Draft project roadmap.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.High,
    RequiredHours = 2.6,
    DueDate = new DateTime(2025, 5, 20, 9, 0, 0),
    Category = "Work"
},

new TaskModel
{
    Id = 27,
    Title = "Write social post copy",
    Description = "Write social post copy.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 2.1,
    DueDate = new DateTime(2025, 5, 20, 12, 0, 0),
    Category = "Marketing"
},

new TaskModel
{
    Id = 28,
    Title = "Update onboarding materials",
    Description = "Update onboarding materials.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 3.7,
    DueDate = new DateTime(2025, 5, 20, 15, 0, 0),
    Category = "Work"
},

new TaskModel
{
    Id = 29,
    Title = "Plan sprint backlog",
    Description = "Plan sprint backlog.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 3.3,
    DueDate = new DateTime(2025, 5, 20, 18, 0, 0),
    Category = "Work"
},

new TaskModel
{
    Id = 30,
    Title = "Review quarterly budget",
    Description = "Review quarterly budget.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.High,
    RequiredHours = 4.0,
    DueDate = new DateTime(2025, 5, 20, 21, 0, 0),
    Category = "Work"
},

new TaskModel
{
    Id = 31,
    Title = "Stretching routine",
    Description = "Stretching routine.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 2.2,
    DueDate = new DateTime(2025, 5, 21, 0, 0, 0),
    Category = "Fitness"
},

new TaskModel
{
    Id = 32,
    Title = "Brainstorm article ideas",
    Description = "Brainstorm article ideas.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.High,
    RequiredHours = 3.7,
    DueDate = new DateTime(2025, 5, 21, 3, 0, 0),
    Category = "Writing"
},

new TaskModel
{
    Id = 33,
    Title = "Watch tutorial videos",
    Description = "Watch tutorial videos.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Low,
    RequiredHours = 3.4,
    DueDate = new DateTime(2025, 5, 21, 6, 0, 0),
    Category = "Learning"
},

new TaskModel
{
    Id = 34,
    Title = "Write social post copy",
    Description = "Write social post copy.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 1.1,
    DueDate = new DateTime(2025, 5, 21, 9, 0, 0),
    Category = "Marketing"
},

new TaskModel
{
    Id = 35,
    Title = "Grocery shopping",
    Description = "Grocery shopping.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Low,
    RequiredHours = 2.4,
    DueDate = new DateTime(2025, 5, 21, 12, 0, 0),
    Category = "Personal"
},

new TaskModel
{
    Id = 36,
    Title = "Write social post copy",
    Description = "Write social post copy.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.High,
    RequiredHours = 2.9,
    DueDate = new DateTime(2025, 5, 21, 15, 0, 0),
    Category = "Marketing"
},

new TaskModel
{
    Id = 37,
    Title = "Implement unit tests",
    Description = "Implement unit tests.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 2.1,
    DueDate = new DateTime(2025, 5, 21, 18, 0, 0),
    Category = "Development"
},

new TaskModel
{
    Id = 38,
    Title = "Brainstorm article ideas",
    Description = "Brainstorm article ideas.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 3.7,
    DueDate = new DateTime(2025, 5, 21, 21, 0, 0),
    Category = "Writing"
},

new TaskModel
{
    Id = 39,
    Title = "Cycle 15km",
    Description = "Cycle 15km.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.High,
    RequiredHours = 3.0,
    DueDate = new DateTime(2025, 5, 22, 0, 0, 0),
    Category = "Fitness"
},

new TaskModel
{
    Id = 40,
    Title = "Analyze SEO metrics",
    Description = "Analyze SEO metrics.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 2.0,
    DueDate = new DateTime(2025, 5, 22, 3, 0, 0),
    Category = "Marketing"
},

new TaskModel
{
    Id = 41,
    Title = "Refactor authentication module",
    Description = "Refactor authentication module.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.High,
    RequiredHours = 3.3,
    DueDate = new DateTime(2025, 5, 22, 6, 0, 0),
    Category = "Development"
},

new TaskModel
{
    Id = 42,
    Title = "Code review pull requests",
    Description = "Code review pull requests.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 3.1,
    DueDate = new DateTime(2025, 5, 22, 9, 0, 0),
    Category = "Development"
},

new TaskModel
{
    Id = 43,
    Title = "Analyze SEO metrics",
    Description = "Analyze SEO metrics.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 1.7,
    DueDate = new DateTime(2025, 5, 22, 12, 0, 0),
    Category = "Marketing"
},

new TaskModel
{
    Id = 44,
    Title = "Write newsletter draft",
    Description = "Write newsletter draft.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 2.2,
    DueDate = new DateTime(2025, 5, 22, 15, 0, 0),
    Category = "Writing"
},

new TaskModel
{
    Id = 45,
    Title = "Pay utility bills",
    Description = "Pay utility bills.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.High,
    RequiredHours = 1.8,
    DueDate = new DateTime(2025, 5, 22, 18, 0, 0),
    Category = "Personal"
},

new TaskModel
{
    Id = 46,
    Title = "Fix UI responsive issues",
    Description = "Fix UI responsive issues.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Low,
    RequiredHours = 2.6,
    DueDate = new DateTime(2025, 5, 22, 21, 0, 0),
    Category = "Development"
},

new TaskModel
{
    Id = 47,
    Title = "Read research paper",
    Description = "Read research paper.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Low,
    RequiredHours = 3.5,
    DueDate = new DateTime(2025, 5, 23, 0, 0, 0),
    Category = "Learning"
},

new TaskModel
{
    Id = 48,
    Title = "Cycle 15km",
    Description = "Cycle 15km.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.High,
    RequiredHours = 1.2,
    DueDate = new DateTime(2025, 5, 23, 3, 0, 0),
    Category = "Fitness"
},

new TaskModel
{
    Id = 49,
    Title = "Watch tutorial videos",
    Description = "Watch tutorial videos.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Low,
    RequiredHours = 2.6,
    DueDate = new DateTime(2025, 5, 23, 6, 0, 0),
    Category = "Learning"
},

new TaskModel
{
    Id = 50,
    Title = "Outline e‑book chapter",
    Description = "Outline e‑book chapter.",
    TaskOrder = 0,
    Status = TaskStatus.Open,
    Priority = TaskPriority.Medium,
    RequiredHours = 2.0,
    DueDate = new DateTime(2025, 5, 23, 9, 0, 0),
    Category = "Writing"
}
        };
    }
}
