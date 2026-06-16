namespace CommonVoice.API.Application.UseCases.Protests;

using CommonVoice.API.Application.Events;
using CommonVoice.API.Domain.Aggregates.Protests;
using CommonVoice.API.Domain.Repositories;
using CommonVoice.API.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

public sealed record CreateProtestCommand(
    Guid     OrganiserId,
    string   Title,
    string?  Description,
    string   Province,
    string   Area,
    string   GrievanceCategory,
    string   Sector,
    double   Latitude,
    double   Longitude,
    int      RadiusMetres,
    DateTime ScheduledAt);

public sealed record CreateProtestResult(Guid ProtestId);

public sealed class CreateProtestUseCase(
    IProtestRepository     protests,
    IDomainEventDispatcher events,
    ILogger<CreateProtestUseCase> logger)
{
    public async Task<CreateProtestResult> ExecuteAsync(CreateProtestCommand cmd, CancellationToken ct = default)
    {
        logger.LogDebug("Creating protest for organiser {OrganiserId}", cmd.OrganiserId);

        var protest = Protest.Create(
            cmd.OrganiserId,
            cmd.Title,
            cmd.Description,
            new Province(cmd.Province),
            cmd.Area,
            new GrievanceCategory(cmd.GrievanceCategory),
            cmd.Sector,
            new GeoPoint(cmd.Latitude, cmd.Longitude),
            cmd.RadiusMetres,
            cmd.ScheduledAt);

        await protests.AddAsync(protest, ct);
        await protests.SaveChangesAsync(ct);
        await events.DispatchAndClearAsync(protest, ct);

        logger.LogInformation("Protest created — {ProtestId}", protest.Id);
        return new CreateProtestResult(protest.Id);
    }
}
