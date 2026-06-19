namespace CommonVoice.API.Application.Events;

using CommonVoice.API.Domain.Shared;

public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken ct = default);
}
