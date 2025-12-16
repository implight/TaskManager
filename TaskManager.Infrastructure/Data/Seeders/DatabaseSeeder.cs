using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Infrastructure.Data.Seeders
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!await context.TaskTypes.AnyAsync())
            {
                await SeedTaskTypesAsync(context);
                await context.SaveChangesAsync();
            }

            if (!await context.Tasks.IgnoreQueryFilters().AnyAsync())
            {
                await SeedTasksAsync(context);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedTaskTypesAsync(AppDbContext context)
        {
            var taskTypes = new[]
            {
                TaskType.Create("Development"),
                TaskType.Create("Testing"),
                TaskType.Create("Design"),
                TaskType.Create("Documentation"),
                TaskType.Create("Deployment"),
                TaskType.Create("Maintenance"),
                TaskType.Create("Meeting"),
                TaskType.Create("Research")
            };

            await context.TaskTypes.AddRangeAsync(taskTypes);
        }

        private static async Task SeedTasksAsync(AppDbContext context)
        {
            var taskTypes = await context.TaskTypes.ToListAsync();
            var random = new Random();

            var tasks = new List<Domain.Entities.Task>();

            for (int i = 1; i <= 50; i++)
            {
                var taskType = taskTypes[random.Next(taskTypes.Count)];
                var status = (TaskStatus)random.Next(1, 6);

                var task = Domain.Entities.Task.Create(
                    $"Task {i}: {taskType.Name}",
                    $"Description for task {i}",
                    taskType);

                var createdDate = DateTime.UtcNow.AddDays(-random.Next(1, 30));
                task.GetType().GetProperty("CreatedAt")?.SetValue(task, createdDate);

                if (status != TaskStatus.Pending)
                {
                    var statusDate = createdDate.AddHours(random.Next(1, 24));
                    task.GetType().GetProperty("StatusAt")?.SetValue(task, statusDate);
                }

                tasks.Add(task);
            }

            await context.Tasks.AddRangeAsync(tasks);
        }
    }
}
