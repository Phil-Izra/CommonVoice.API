namespace CommonVoice.API.Application.UseCases.Participants;

using CommonVoice.API.Application.Events;
using CommonVoice.API.Domain.Aggregates.Participants;
using CommonVoice.API.Domain.Repositories;
using CommonVoice.API.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

public sealed record CheckInParticipantCommand(
    string JoinToken,
    string DeviceFingerprint,
    double Latitude,
    double Longitude);

public sealed record CheckInParticipantResult(Guid ParticipantId, bool IsWithinBoundary);

public sealed class CheckInParticipantUseCase(
    IProtestRepository     protests,
    IParticipantRepository participants,
    IDomainEventDispatcher events,
    ILogger<CheckInParticipantUseCase> logger)
{
    public async Task<CheckInParticipantResult> ExecuteAsync(
        CheckInParticipantCommand cmd, CancellationToken ct = default)
    {
        var protest = await protests.GetByJoinTokenAsync(cmd.JoinToken, ct)
            ?? throw new KeyNotFoundException("Invalid or expired join token");

        if (!protest.IsJoinTokenValid(cmd.JoinToken))
            throw new InvalidOperationException("Join token has expired or protest is not active");

        if (await participants.ExistsAsync(protest.Id, cmd.DeviceFingerprint, ct))
            throw new InvalidOperationException("This device has already checked in to the protest");

        var participant = Participant.CheckIn(
            protest.Id,
            cmd.DeviceFingerprint,
            new GeoPoint(cmd.Latitude, cmd.Longitude),
            protest.Location,
            protest.RadiusMetres);

        await participants.AddAsync(participant, ct);
        await participants.SaveChangesAsync(ct);
        await events.DispatchAndClearAsync(participant, ct);

        logger.LogInformation(
            "Participant {ParticipantId} checked in to protest {ProtestId} — within boundary: {WithinBoundary}",
            participant.Id, protest.Id, participant.IsWithinBoundary);

        return new CheckInParticipantResult(participant.Id, participant.IsWithinBoundary);
    }
}
