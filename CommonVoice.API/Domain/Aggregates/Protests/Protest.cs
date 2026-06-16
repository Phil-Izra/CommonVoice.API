namespace CommonVoice.API.Domain.Aggregates.Protests;

using CommonVoice.API.Domain.Shared;
using CommonVoice.API.Domain.ValueObjects;

public enum ProtestStatus { Draft, Scheduled, Active, Completed, Cancelled }

public class Protest : AggregateRoot
{
    public Guid              OrganiserId        { get; private set; }
    public string            Title              { get; private set; } = default!;
    public string?           Description        { get; private set; }
    public Province          Province           { get; private set; } = default!;
    public string            Area               { get; private set; } = default!;
    public GrievanceCategory GrievanceCategory  { get; private set; } = default!;
    public string            Sector             { get; private set; } = default!;
    public ProtestStatus     Status             { get; private set; }
    public GeoPoint          Location           { get; private set; } = default!;
    public int               RadiusMetres       { get; private set; }
    public DateTime          ScheduledAt        { get; private set; }
    public DateTime?         StartedAt          { get; private set; }
    public DateTime?         EndedAt            { get; private set; }
    public string?           JoinToken          { get; private set; }
    public DateTime?         JoinTokenExpires   { get; private set; }
    public DateTime          CreatedAt          { get; private set; }

    private readonly List<Demand> _demands = [];
    public IReadOnlyList<Demand> Demands => _demands.AsReadOnly();

    private Protest() { }

    public static Protest Create(
        Guid organiserId,
        string title,
        string? description,
        Province province,
        string area,
        GrievanceCategory grievanceCategory,
        string sector,
        GeoPoint location,
        int radiusMetres,
        DateTime scheduledAt)
    {
        if (radiusMetres is < 100 or > 10_000)
            throw new ArgumentOutOfRangeException(nameof(radiusMetres), "Radius must be 100–10,000 metres");

        if (scheduledAt < DateTime.UtcNow)
            throw new ArgumentException("Protest must be scheduled in the future");

        var p = new Protest
        {
            OrganiserId       = organiserId,
            Title             = title.Trim(),
            Description       = description?.Trim(),
            Province          = province,
            Area              = area.Trim(),
            GrievanceCategory = grievanceCategory,
            Sector            = sector.Trim(),
            Status            = ProtestStatus.Draft,
            Location          = location,
            RadiusMetres      = radiusMetres,
            ScheduledAt       = scheduledAt,
            CreatedAt         = DateTime.UtcNow,
        };

        p.Raise(new ProtestCreatedEvent(p.Id, p.Province.Value, p.GrievanceCategory.Value));
        return p;
    }

    public void AddDemand(string description)
    {
        if (Status is not (ProtestStatus.Draft or ProtestStatus.Scheduled))
            throw new InvalidOperationException("Cannot add demands to an active or completed protest");

        _demands.Add(Demand.Create(Id, _demands.Count + 1, description));
    }

    public string Activate()
    {
        if (Status is not (ProtestStatus.Draft or ProtestStatus.Scheduled))
            throw new InvalidOperationException($"Cannot activate a protest in {Status} status");

        if (_demands.Count == 0)
            throw new InvalidOperationException("A protest must have at least one demand before activation");

        Status           = ProtestStatus.Active;
        StartedAt        = DateTime.UtcNow;
        JoinToken        = GenerateToken();
        JoinTokenExpires = DateTime.UtcNow.AddHours(12);

        Raise(new ProtestActivatedEvent(Id, JoinToken));
        return JoinToken;
    }

    public void Complete()
    {
        if (Status != ProtestStatus.Active)
            throw new InvalidOperationException("Only active protests can be completed");

        Status    = ProtestStatus.Completed;
        EndedAt   = DateTime.UtcNow;
        JoinToken = null;

        Raise(new ProtestCompletedEvent(Id, Province.Value, GrievanceCategory.Value));
    }

    public bool IsJoinTokenValid(string token) =>
        JoinToken == token &&
        JoinTokenExpires.HasValue &&
        JoinTokenExpires.Value > DateTime.UtcNow &&
        Status == ProtestStatus.Active;

    private static string GenerateToken() =>
        Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("+", "-").Replace("/", "_").Replace("=", "")[..16];
}
