namespace CommonVoice.API.Application.UseCases.Protests;

using CommonVoice.API.Application.Events;
using CommonVoice.API.Domain.Repositories;
using Microsoft.Extensions.Logging;

public sealed record CompleteProtestCommand(Guid ProtestId, Guid OrganiserId);

public sealed class CompleteProtestUseCase(
    IProtestRepository     protests,
    IDomainEventDispatcher events,
    ILogger<CompleteProtestUseCase> logger)
{
    public async Task ExecuteAsync(CompleteProtestCommand cmd, CancellationToken ct = default)
    {
        var protest = await protests.GetByIdAsync(cmd.ProtestId, ct)
            ?? throw new KeyNotFoundException($"Protest {cmd.ProtestId} not found");

        if (protest.OrganiserId != cmd.OrganiserId)
            throw new UnauthorizedAccessException("Only the organiser can complete the protest");

        protest.Complete();

        await protests.SaveChangesAsync(ct);
        await events.DispatchAndClearAsync(protest, ct);

        logger.LogInformation("Protest completed — {ProtestId}", cmd.ProtestId);
    }
}
