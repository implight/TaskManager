namespace TaskManager.Domain.Events.TaskTypes
{
    public class TaskTypeCreatedEvent : DomainEvent
    {
        public Guid TaskTypeId { get; }
        public string Name { get; }

        public TaskTypeCreatedEvent(Guid taskTypeId, string name)
        {
            TaskTypeId = taskTypeId;
            Name = name;
        }
    }
}
