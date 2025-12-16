using TaskManager.Domain.Enums;
using TaskManager.Domain.Events;
using TaskManager.Domain.Events.Tasks;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Interfaces;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Domain.Entities
{
    public class Task : IAggregateRoot
    {
        private readonly List<DomainEvent> _domainEvents = new();

        public Guid Id { get; private set; }
        public String Name { get; private set; } = null!;
        public String? Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public TaskStatus Status { get; private set; }
        public DateTime StatusAt { get; private set; }
        public Guid TaskTypeId { get; private set; }
        public TaskType TaskType { get; private set; } = null!;

        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        private Task() { }

        public static Task Create(String name, String? description, TaskType taskType)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new TaskDomainException("Task name cannot be empty.");

            var now = DateTime.UtcNow;

            var task = new Task
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
                Description = description?.Trim(),
                Status = TaskStatus.Pending,
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatedAt = now,
                StatusAt = now
            };

            task._domainEvents.Add(new TaskCreatedEvent(
                task.Id,
                name,
                taskType.Id));

            return task;
        }

        public void UpdateName(String newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new TaskDomainException("Task name cannot be empty.");

            newName = newName.Trim();

            if (Name == newName) return;

            var oldName = Name;
            Name = newName;

            _domainEvents.Add(new TaskUpdatedEvent(
                Id,
                oldName,
                newName,
                Description,
                Description));
        }

        public void UpdateDescription(String? newDescription)
        {
            newDescription = newDescription?.Trim();

            if (Description == newDescription) return;

            var oldDescription = Description;
            Description = newDescription;

            _domainEvents.Add(new TaskUpdatedEvent(
                Id,
                Name,
                Name,
                oldDescription,
                newDescription));
        }

        public void Run()
        {
            ChangeStatus(TaskStatus.Running);
        }

        public void Pause()
        {
            ChangeStatus(TaskStatus.Paused);
        }

        public void Cancel()
        {
            ChangeStatus(TaskStatus.Cancelled);
        }

        public void Complete()
        {
            ChangeStatus(TaskStatus.Completed);
        }

        private void ChangeStatus(TaskStatus newStatus)
        {
            if (Status == newStatus)
                return;

            if (Status.IsFinalState())
                throw TaskDomainException.CannotModifyFinalState();

            if (!Status.CanTransitionTo(newStatus))
                throw TaskDomainException.InvalidStatusTransition(Status, newStatus);

            var oldStatus = Status;
            Status = newStatus;
            StatusAt = DateTime.UtcNow;

            _domainEvents.Add(new TaskStatusChangedEvent(
                Id,
                oldStatus,
                newStatus));
        }

        public void Delete()
        {
            if (Status == TaskStatus.Running)
                throw new TaskDomainException("Cannot delete running task.");

            _domainEvents.Add(new TaskDeletedEvent(Id));
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
