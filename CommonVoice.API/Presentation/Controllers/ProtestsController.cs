namespace CommonVoice.API.Presentation.Controllers;

using System.Security.Claims;
using CommonVoice.API.Application.UseCases.Protests;
using CommonVoice.API.Presentation.Models.Protests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/protests")]
[Authorize]
public sealed class ProtestsController(
    CreateProtestUseCase        createProtest,
    AddDemandUseCase            addDemand,
    ActivateProtestUseCase      activateProtest,
    CompleteProtestUseCase      completeProtest,
    ILogger<ProtestsController> logger) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProtestRequest req, CancellationToken ct)
    {
        var organiserId = CurrentUserId;
        logger.LogDebug("Create protest request from organiser {OrganiserId}", organiserId);

        var result = await createProtest.ExecuteAsync(new CreateProtestCommand(
            organiserId, req.Title, req.Description, req.Province, req.Area,
            req.GrievanceCategory, req.Sector, req.Latitude, req.Longitude,
            req.RadiusMetres, req.ScheduledAt), ct);

        return CreatedAtAction(nameof(Create), new { result.ProtestId }, result);
    }

    [HttpPost("{id:guid}/demands")]
    public async Task<IActionResult> AddDemand(Guid id, [FromBody] AddDemandRequest req, CancellationToken ct)
    {
        logger.LogDebug("Add demand to protest {ProtestId}", id);

        await addDemand.ExecuteAsync(new AddDemandCommand(id, CurrentUserId, req.Description), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Activate protest {ProtestId}", id);

        var result = await activateProtest.ExecuteAsync(new ActivateProtestCommand(id, CurrentUserId), ct);
        return Ok(result);
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Complete protest {ProtestId}", id);

        await completeProtest.ExecuteAsync(new CompleteProtestCommand(id, CurrentUserId), ct);
        return NoContent();
    }
}
