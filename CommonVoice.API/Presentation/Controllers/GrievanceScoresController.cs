namespace CommonVoice.API.Presentation.Controllers;

using CommonVoice.API.Application.UseCases.GrievanceScores;
using CommonVoice.API.Presentation.Models.GrievanceScores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/grievance-scores")]
[Authorize(Roles = "Admin")]
public sealed class GrievanceScoresController(
    CalculateProvincialGwsUseCase        calculateGws,
    ILogger<GrievanceScoresController>   logger) : ControllerBase
{
    [HttpPost("calculate")]
    public async Task<IActionResult> Calculate([FromBody] CalculateGwsRequest req, CancellationToken ct)
    {
        logger.LogInformation("GWS calculation triggered for {Month}/{Year}", req.Month, req.Year);

        var result = await calculateGws.ExecuteAsync(
            new CalculateProvincialGwsCommand(req.Month, req.Year), ct);

        return Ok(result);
    }
}
