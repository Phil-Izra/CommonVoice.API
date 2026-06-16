namespace CommonVoice.API.Application.UseCases.Protests;

using CommonVoice.API.Application.Events;
using CommonVoice.API.Domain.Repositories;
using Microsoft.Extensions.Logging;

public sealed record ActivateProtestCommand(Guid ProtestId, Guid OrganiserId);

public sealed record ActivateProtestResult(string JoinToken);

public sealed class ActivateProtestUseCase(
    IProtestRepository     protests,
    IDomainEventDispatcher events,
    ILogger<ActivateProtestUseCase> logger)
{
    public async Task<ActivateProtestResult> ExecuteAsync(ActivateProtestCommand cmd, CancellationToken ct = default)
    {
        var protest = await protests.GetByIdAsync(cmd.ProtestId, ct)
            ?? throw new KeyNotFoundException($"Protest {cmd.ProtestId} not found");

        if (protest.OrganiserId != cmd.OrganiserId)
            throw new UnauthorizedAccessException("Only the organiser can activate the protest");

        var joinToken = protest.Activate();

        await protests.SaveChangesAsync(ct);
        await events.DispatchAndClearAsync(protest, ct);

        logger.LogInformation("Protest activated — {ProtestId}", cmd.ProtestId);
        return new ActivateProtestResult(joinToken);
    }
}
