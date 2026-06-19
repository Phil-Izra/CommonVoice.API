namespace CommonVoice.API.Application.Events.Handlers;

using CommonVoice.API.Domain.Aggregates.Protests;
using Microsoft.Extensions.Logging;

public sealed class ProtestCreatedEventHandler(ILogger<ProtestCreatedEventHandler> logger)
    : IDomainEventHandler<ProtestCreatedEvent>
{
    public Task HandleAsync(ProtestCreatedEvent e, CancellationToken ct = default)
    {
        logger.LogInformation(
            "Protest created — Id: {ProtestId}, Province: {Province}, Category: {Category}",
            e.ProtestId, e.Province, e.GrievanceCategory);
        return Task.CompletedTask;
    }
}
