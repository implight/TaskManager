
namespace TaskManager.Domain.Enums
{
    public enum TaskStatus
    {
        Pending = 1,
        Running = 2,
        Paused = 3,
        Cancelled = 4,
        Completed = 5
    }

    public static class TaskStatusExtensions
    {
        public static string ToFriendlyString(this TaskStatus status)
        {
            return status switch
            {
                TaskStatus.Pending => "Pending",
                TaskStatus.Running => "Running",
                TaskStatus.Paused => "Paused",
                TaskStatus.Cancelled => "Cancelled",
                TaskStatus.Completed => "Completed",
                _ => "Unknown"
            };
        }

        public static bool CanTransitionTo(this TaskStatus from, TaskStatus to)
        {
            var validTransitions = new Dictionary<TaskStatus, List<TaskStatus>>
            {
                [TaskStatus.Pending] = new() { TaskStatus.Running, TaskStatus.Cancelled },
                [TaskStatus.Running] = new() { TaskStatus.Paused, TaskStatus.Completed, TaskStatus.Cancelled },
                [TaskStatus.Paused] = new() { TaskStatus.Running, TaskStatus.Cancelled },
                [TaskStatus.Cancelled] = new() { }, 
                [TaskStatus.Completed] = new() { } 
            };

            return validTransitions[from].Contains(to);
        }

        public static bool IsFinalState(this TaskStatus status)
        {
            return status == TaskStatus.Cancelled || status == TaskStatus.Completed;
        }
    }
}
