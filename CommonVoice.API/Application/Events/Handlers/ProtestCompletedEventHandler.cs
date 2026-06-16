namespace CommonVoice.API.Application.Events.Handlers;

using CommonVoice.API.Domain.Aggregates.Protests;
using Microsoft.Extensions.Logging;

public sealed class ProtestCompletedEventHandler(ILogger<ProtestCompletedEventHandler> logger)
    : IDomainEventHandler<ProtestCompletedEvent>
{
    public Task HandleAsync(ProtestCompletedEvent e, CancellationToken ct = default)
    {
        logger.LogInformation(
            "Protest completed — Id: {ProtestId}, Province: {Province}, Category: {Category}",
            e.ProtestId, e.Province, e.GrievanceCategory);
        return Task.CompletedTask;
    }
}
