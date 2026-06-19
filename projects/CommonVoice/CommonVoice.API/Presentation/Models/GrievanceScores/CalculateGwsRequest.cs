namespace CommonVoice.API.Presentation.Models.GrievanceScores;

using System.ComponentModel.DataAnnotations;

public sealed record CalculateGwsRequest(
    [Range(1, 12)]    int Month,
    [Range(2000, 9999)] int Year
);
