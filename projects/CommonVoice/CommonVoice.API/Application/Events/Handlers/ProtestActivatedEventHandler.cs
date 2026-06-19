namespace CommonVoice.API.Application.Events.Handlers;

using CommonVoice.API.Domain.Aggregates.Protests;
using Microsoft.Extensions.Logging;

public sealed class ProtestActivatedEventHandler(ILogger<ProtestActivatedEventHandler> logger)
    : IDomainEventHandler<ProtestActivatedEvent>
{
    public Task HandleAsync(ProtestActivatedEvent e, CancellationToken ct = default)
    {
        logger.LogInformation(
            "Protest activated — Id: {ProtestId}, JoinToken issued",
            e.ProtestId);
        return Task.CompletedTask;
    }
}
