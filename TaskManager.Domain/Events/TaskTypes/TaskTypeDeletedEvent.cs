namespace TaskManager.Domain.Events.TaskTypes
{
    public class TaskTypeDeletedEvent : DomainEvent
    {
        public Guid TaskTypeId { get; }

        public TaskTypeDeletedEvent(Guid taskTypeId)
        {
            TaskTypeId = taskTypeId;
        }
    }
}
