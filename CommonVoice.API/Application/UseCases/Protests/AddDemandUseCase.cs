namespace CommonVoice.API.Application.UseCases.Protests;

using CommonVoice.API.Domain.Repositories;
using Microsoft.Extensions.Logging;

public sealed record AddDemandCommand(Guid ProtestId, Guid OrganiserId, string Description);

public sealed class AddDemandUseCase(
    IProtestRepository protests,
    ILogger<AddDemandUseCase> logger)
{
    public async Task ExecuteAsync(AddDemandCommand cmd, CancellationToken ct = default)
    {
        var protest = await protests.GetByIdAsync(cmd.ProtestId, ct)
            ?? throw new KeyNotFoundException($"Protest {cmd.ProtestId} not found");

        if (protest.OrganiserId != cmd.OrganiserId)
            throw new UnauthorizedAccessException("Only the organiser can add demands");

        protest.AddDemand(cmd.Description);

        await protests.SaveChangesAsync(ct);

        logger.LogInformation("Demand added to protest {ProtestId}", cmd.ProtestId);
    }
}
