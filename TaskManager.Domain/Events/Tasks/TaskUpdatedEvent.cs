namespace TaskManager.Domain.Events.Tasks
{
    public class TaskUpdatedEvent : DomainEvent
    {
        public Guid TaskId { get; }
        public string OldName { get; }
        public string NewName { get; }
        public string? OldDescription { get; }
        public string? NewDescription { get; }

        public TaskUpdatedEvent(
            Guid taskId,
            string oldName,
            string newName,
            string? oldDescription,
            string? newDescription)
        {
            TaskId = taskId;
            OldName = oldName;
            NewName = newName;
            OldDescription = oldDescription;
            NewDescription = newDescription;
        }
    }
}
