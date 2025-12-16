
using TaskManager.Domain.Events;

namespace TaskManager.Domain.Interfaces
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
