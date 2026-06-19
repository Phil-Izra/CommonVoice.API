namespace CommonVoice.API.Domain.Aggregates.Participants;

using CommonVoice.API.Domain.Shared;
using CommonVoice.API.Domain.ValueObjects;

public class Participant : AggregateRoot
{
    public Guid     ProtestId         { get; private set; }
    public string   DeviceFingerprint { get; private set; } = default!;
    public GeoPoint Location          { get; private set; } = default!;
    public bool     IsWithinBoundary  { get; private set; }
    public DateTime CheckedInAt       { get; private set; }

    private Participant() { }

    public static Participant CheckIn(
        Guid protestId,
        string deviceFingerprint,
        GeoPoint location,
        GeoPoint protestCentre,
        int radiusMetres)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deviceFingerprint);

        var isWithin = location.DistanceMetresTo(protestCentre) <= radiusMetres;

        var p = new Participant
        {
            ProtestId         = protestId,
            DeviceFingerprint = deviceFingerprint,
            Location          = location,
            IsWithinBoundary  = isWithin,
            CheckedInAt       = DateTime.UtcNow,
        };

        p.Raise(new ParticipantCheckedInEvent(protestId, isWithin));
        return p;
    }
}

public record ParticipantCheckedInEvent(Guid ProtestId, bool IsWithinBoundary)
    : IDomainEvent { public DateTime OccurredAt => DateTime.UtcNow; }
