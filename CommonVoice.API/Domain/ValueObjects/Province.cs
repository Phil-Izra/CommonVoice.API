namespace CommonVoice.API.Domain.ValueObjects;

public record Province
{
    public static readonly string[] Valid =
    [
        "gauteng", "western_cape", "kwazulu_natal", "eastern_cape",
        "limpopo", "mpumalanga", "north_west", "free_state", "northern_cape"
    ];

    public string Value { get; }

    public Province(string value)
    {
        if (!Valid.Contains(value.ToLower()))
            throw new ArgumentException($"Invalid province: {value}");
        Value = value.ToLower();
    }

    public override string ToString() => Value;
}
