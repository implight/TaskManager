namespace TaskManager.Domain.Events.Tasks
{
    public class TaskDeletedEvent : DomainEvent
    {
        public Guid TaskId { get; }
        
        public TaskDeletedEvent(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}
