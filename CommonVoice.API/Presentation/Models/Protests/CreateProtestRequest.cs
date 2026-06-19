namespace CommonVoice.API.Presentation.Models.Protests;

using System.ComponentModel.DataAnnotations;

public sealed record CreateProtestRequest(
    [Required] string   Title,
    string?             Description,
    [Required] string   Province,
    [Required] string   Area,
    [Required] string   GrievanceCategory,
    [Required] string   Sector,
    double              Latitude,
    double              Longitude,
    [Range(1, 50_000)] int RadiusMetres,
    DateTime            ScheduledAt
);
