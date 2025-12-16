namespace TaskManager.Domain.Events.Tasks
{
    public class TaskCreatedEvent : DomainEvent
    {
        public Guid TaskId { get; }
        public string TaskName { get; }
        public Guid TaskTypeId { get; }

        public TaskCreatedEvent(Guid taskId, string taskName, Guid taskTypeId)
        {
            TaskId = taskId;
            TaskName = taskName;
            TaskTypeId = taskTypeId;
        }
    }
}
