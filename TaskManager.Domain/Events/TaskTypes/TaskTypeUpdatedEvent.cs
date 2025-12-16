namespace TaskManager.Domain.Events.TaskTypes
{
    public class TaskTypeUpdatedEvent : DomainEvent
    {
        public Guid TaskTypeId { get; }
        public string OldName { get; }
        public string NewName { get; }

        public TaskTypeUpdatedEvent(Guid taskTypeId, string oldName, string newName)
        {
            TaskTypeId = taskTypeId;
            OldName = oldName;
            NewName = newName;
        }
    }
}
