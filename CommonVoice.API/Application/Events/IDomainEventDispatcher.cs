namespace CommonVoice.API.Application.Events;

using CommonVoice.API.Domain.Shared;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearAsync(Entity entity, CancellationToken ct = default);
}
