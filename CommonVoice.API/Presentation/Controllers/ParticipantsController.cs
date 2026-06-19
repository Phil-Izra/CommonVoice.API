namespace CommonVoice.API.Presentation.Controllers;

using CommonVoice.API.Application.UseCases.Participants;
using CommonVoice.API.Presentation.Models.Participants;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/participants")]
public sealed class ParticipantsController(
    CheckInParticipantUseCase       checkIn,
    ILogger<ParticipantsController> logger) : ControllerBase
{
    [HttpPost("check-in")]
    public async Task<IActionResult> CheckIn([FromBody] CheckInRequest req, CancellationToken ct)
    {
        logger.LogDebug("Check-in request with join token {JoinToken}", req.JoinToken);

        var result = await checkIn.ExecuteAsync(new CheckInParticipantCommand(
            req.JoinToken, req.DeviceFingerprint, req.Latitude, req.Longitude), ct);

        return Ok(result);
    }
}
