namespace CommonVoice.API.Domain.Aggregates.GrievanceScores;

using CommonVoice.API.Domain.Shared;
using CommonVoice.API.Domain.ValueObjects;

// Created by the nightly background job — never by direct user action
public class GrievanceScore : AggregateRoot
{
    public Guid              ProtestId         { get; private set; }
    public Province          Province          { get; private set; } = default!;
    public GrievanceCategory GrievanceCategory { get; private set; } = default!;
    public int               ScoreMonth        { get; private set; }
    public int               ScoreYear         { get; private set; }
    public GwsScore          Score             { get; private set; } = default!;
    public int               ParticipantCount  { get; private set; }
    public DateTime          CalculatedAt      { get; private set; }

    private GrievanceScore() { }

    public static GrievanceScore Calculate(
        Guid protestId,
        Province province,
        GrievanceCategory category,
        GwsScore score,
        int participantCount)
    {
        if (participantCount < 0)
            throw new ArgumentOutOfRangeException(nameof(participantCount));

        var now = DateTime.UtcNow;
        return new GrievanceScore
        {
            ProtestId         = protestId,
            Province          = province,
            GrievanceCategory = category,
            ScoreMonth        = now.Month,
            ScoreYear         = now.Year,
            Score             = score,
            ParticipantCount  = participantCount,
            CalculatedAt      = now,
        };
    }
}
