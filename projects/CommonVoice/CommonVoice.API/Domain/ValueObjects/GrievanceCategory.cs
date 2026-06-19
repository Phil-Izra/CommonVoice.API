namespace CommonVoice.API.Domain.ValueObjects;

public record GrievanceCategory
{
    public static readonly string[] Valid =
    [
        "housing", "unemployment", "load_shedding", "water_access",
        "education", "healthcare", "safety_crime", "infrastructure",
        "labour_rights", "illegal_immigration", "other"
    ];

    public string Value { get; }

    public GrievanceCategory(string value)
    {
        if (!Valid.Contains(value.ToLower()))
            throw new ArgumentException($"Invalid grievance category: {value}");
        Value = value.ToLower();
    }

    public override string ToString() => Value;
}
