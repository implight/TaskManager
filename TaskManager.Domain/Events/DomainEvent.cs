
using MediatR;

namespace TaskManager.Domain.Events
{
    public abstract class DomainEvent : INotification
    {
        public DateTime OccurredOn { get; }
        public Guid EventId { get; }

        protected DomainEvent()
        {
            OccurredOn = DateTime.UtcNow;
            EventId = Guid.NewGuid();
        }
    }
}
