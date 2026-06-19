namespace CommonVoice.API.Infrastructure.Events;

using CommonVoice.API.Application.Events;
using CommonVoice.API.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public sealed class DomainEventDispatcher(
    IServiceProvider provider,
    ILogger<DomainEventDispatcher> logger) : IDomainEventDispatcher
{
    public async Task DispatchAndClearAsync(Entity entity, CancellationToken ct = default)
    {
        foreach (var domainEvent in entity.DomainEvents)
        {
            var eventType   = domainEvent.GetType();
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
            var handlers    = provider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                var method = handlerType.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync))!;
                await (Task)method.Invoke(handler, [domainEvent, ct])!;
                logger.LogDebug("Dispatched {EventType}", eventType.Name);
            }
        }

        entity.ClearDomainEvents();
    }
}
