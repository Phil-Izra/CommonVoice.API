namespace CommonVoice.API.Domain.Repositories;

using CommonVoice.API.Domain.Aggregates.GrievanceScores;
using CommonVoice.API.Domain.ValueObjects;

public interface IGrievanceScoreRepository
{
    Task<IEnumerable<GrievanceScore>> GetByProvinceAndGrievanceAsync(
        Province province, GrievanceCategory category,
        int month, int year, CancellationToken ct = default);
    Task UpsertAsync(GrievanceScore score, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
