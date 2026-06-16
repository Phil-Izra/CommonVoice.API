namespace CommonVoice.API.Domain.ValueObjects;

// GWS = (M×0.35) + (R×0.30) + (C×0.20) + (F×0.15)
public record GwsScore
{
    public decimal ReachScore       { get; }   // M — participant reach, 0–100
    public decimal RecurrenceScore  { get; }   // R — how often this grievance recurs
    public decimal CrossSectorScore { get; }   // C — breadth across sectors
    public decimal FreshnessScore   { get; }   // F — recency weighting

    public decimal Value =>
        Math.Round(
            ReachScore       * 0.35m +
            RecurrenceScore  * 0.30m +
            CrossSectorScore * 0.20m +
            FreshnessScore   * 0.15m, 2);

    public GwsScore(decimal reach, decimal recurrence, decimal crossSector, decimal freshness)
    {
        Validate(reach,       nameof(reach));
        Validate(recurrence,  nameof(recurrence));
        Validate(crossSector, nameof(crossSector));
        Validate(freshness,   nameof(freshness));

        ReachScore       = reach;
        RecurrenceScore  = recurrence;
        CrossSectorScore = crossSector;
        FreshnessScore   = freshness;
    }

    private static void Validate(decimal v, string name)
    {
        if (v is < 0 or > 100)
            throw new ArgumentOutOfRangeException(name, "Score components must be between 0 and 100");
    }
}
