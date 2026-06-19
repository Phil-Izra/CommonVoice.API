namespace CommonVoice.API.Domain.Services;

using CommonVoice.API.Domain.Aggregates.GrievanceScores;

// Marcher-weighted average: Σ(participants × GWS) ÷ Σ(participants)
public static class ProvincialGwsCalculator
{
    public record Result(decimal ProvincialGws, int TotalParticipants, int ProtestCount);

    public static Result Calculate(IEnumerable<GrievanceScore> scores)
    {
        var list = scores.ToList();
        if (list.Count == 0) return new Result(0, 0, 0);

        var totalParticipants = list.Sum(s => s.ParticipantCount);
        if (totalParticipants == 0) return new Result(0, 0, list.Count);

        var weightedSum = list.Sum(s => (decimal)s.ParticipantCount * s.Score.Value);
        var provincial  = Math.Round(weightedSum / totalParticipants, 2);

        return new Result(provincial, totalParticipants, list.Count);
    }
}
