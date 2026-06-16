namespace CommonVoice.API.Domain.Aggregates.Protests;

using CommonVoice.API.Domain.Shared;

public class Demand : Entity
{
    public Guid      ProtestId      { get; private set; }
    public int       Sequence       { get; private set; }
    public string    Description    { get; private set; } = default!;
    public string    ResponseStatus { get; private set; } = "ignored";
    public DateTime? RespondedAt    { get; private set; }

    private Demand() { }

    internal static Demand Create(Guid protestId, int sequence, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        return new Demand
        {
            ProtestId   = protestId,
            Sequence    = sequence,
            Description = description.Trim(),
        };
    }

    public void UpdateResponse(string status)
    {
        string[] valid = ["ignored", "acknowledged", "in_progress", "resolved"];
        if (!valid.Contains(status))
            throw new ArgumentException($"Invalid response status: {status}");
        ResponseStatus = status;
        RespondedAt    = DateTime.UtcNow;
    }
}
