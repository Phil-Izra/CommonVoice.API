namespace CommonVoice.API.Application.Events.Handlers;

using CommonVoice.API.Domain.Aggregates.Participants;
using Microsoft.Extensions.Logging;

public sealed class ParticipantCheckedInEventHandler(ILogger<ParticipantCheckedInEventHandler> logger)
    : IDomainEventHandler<ParticipantCheckedInEvent>
{
    public Task HandleAsync(ParticipantCheckedInEvent e, CancellationToken ct = default)
    {
        logger.LogInformation(
            "Participant checked in — ProtestId: {ProtestId}, WithinBoundary: {WithinBoundary}",
            e.ProtestId, e.IsWithinBoundary);
        return Task.CompletedTask;
    }
}
