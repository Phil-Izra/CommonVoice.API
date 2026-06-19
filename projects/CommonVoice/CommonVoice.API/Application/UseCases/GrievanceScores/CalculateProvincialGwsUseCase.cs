namespace CommonVoice.API.Application.UseCases.GrievanceScores;

using CommonVoice.API.Domain.Aggregates.GrievanceScores;
using CommonVoice.API.Domain.Repositories;
using CommonVoice.API.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

public sealed record CalculateProvincialGwsCommand(int Month, int Year);

public sealed record CalculateProvincialGwsResult(int ScoresCalculated);

public sealed class CalculateProvincialGwsUseCase(
    IProtestRepository         protests,
    IParticipantRepository     participants,
    IGrievanceScoreRepository  grievanceScores,
    ILogger<CalculateProvincialGwsUseCase> logger)
{
    public async Task<CalculateProvincialGwsResult> ExecuteAsync(
        CalculateProvincialGwsCommand cmd, CancellationToken ct = default)
    {
        logger.LogInformation("Calculating GWS scores for {Month}/{Year}", cmd.Month, cmd.Year);

        var count = 0;

        foreach (var provinceValue in Province.Valid)
        {
            foreach (var categoryValue in GrievanceCategory.Valid)
            {
                var province = new Province(provinceValue);
                var category = new GrievanceCategory(categoryValue);

                var periodProtests = (await protests.GetByProvinceAndGrievanceAsync(
                    province, category, cmd.Month, cmd.Year, ct)).ToList();

                if (periodProtests.Count == 0) continue;

                var distinctSectors = periodProtests.Select(p => p.Sector).Distinct().Count();

                foreach (var protest in periodProtests)
                {
                    var participantCount = await participants.CountByProtestAsync(protest.Id, ct);

                    // Reach: normalize to 100 at 10 000 participants
                    // Recurrence: 5 protests in period = full score
                    // CrossSector: 5 distinct sectors = full score
                    // Freshness: always 100 for current-period data
                    var score = new GwsScore(
                        reach:       Math.Min(participantCount / 10_000m * 100m, 100m),
                        recurrence:  Math.Min(periodProtests.Count * 20m,        100m),
                        crossSector: Math.Min(distinctSectors * 20m,             100m),
                        freshness:   100m);

                    var grievanceScore = GrievanceScore.Calculate(
                        protest.Id, province, category, score, participantCount);

                    await grievanceScores.UpsertAsync(grievanceScore, ct);
                    count++;
                }
            }
        }

        await grievanceScores.SaveChangesAsync(ct);

        logger.LogInformation("GWS calculation complete — {Count} scores upserted", count);
        return new CalculateProvincialGwsResult(count);
    }
}
