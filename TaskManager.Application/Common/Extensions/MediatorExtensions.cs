using MediatR;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Common.Extensions
{
    public static class MediatorExtensions
    {
        public static async Task PublishEventsAndClear(this IMediator mediator, IAggregateRoot aggregateRoot)
        {
            var domainEvents = aggregateRoot.DomainEvents.ToList();
            aggregateRoot.ClearDomainEvents();

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }
}
