namespace CommonVoice.API.Presentation.Models.Participants;

using System.ComponentModel.DataAnnotations;

public sealed record CheckInRequest(
    [Required] string JoinToken,
    [Required] string DeviceFingerprint,
    double            Latitude,
    double            Longitude
);
