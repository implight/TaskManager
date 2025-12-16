using TaskManager.Domain.Events;
using TaskManager.Domain.Events.TaskTypes;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Domain.Entities
{
    public class TaskType : IAggregateRoot
    {
        private readonly List<DomainEvent> _domainEvents = new();

        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;

        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        private TaskType() { }

        public static TaskType Create(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new TaskTypeDomainException("Task type name cannot be empty");

            var taskType = new TaskType
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
            };

            taskType._domainEvents.Add(new TaskTypeCreatedEvent(taskType.Id, taskType.Name));

            return taskType;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new TaskTypeDomainException("Task type name cannot be empty.");

            newName = newName.Trim();

            if (Name == newName) return;

            var oldName = Name;
            Name = newName;

            _domainEvents.Add(new TaskTypeUpdatedEvent(Id, oldName, Name));
        }

        public void Delete()
        {
            _domainEvents.Add(new TaskTypeDeletedEvent(Id));
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
