using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Domain.Events.Tasks
{
    public class TaskStatusChangedEvent : DomainEvent
    {
        public Guid TaskId { get; }
        public TaskStatus OldStatus { get; }
        public TaskStatus NewStatus { get; }

        public TaskStatusChangedEvent(Guid taskId, TaskStatus oldStatus, TaskStatus newStatus)
        {
            TaskId = taskId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
}
