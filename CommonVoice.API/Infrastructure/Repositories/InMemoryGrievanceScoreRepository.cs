namespace CommonVoice.API.Infrastructure.Repositories;

using System.Collections.Concurrent;
using CommonVoice.API.Domain.Aggregates.GrievanceScores;
using CommonVoice.API.Domain.Repositories;
using CommonVoice.API.Domain.ValueObjects;

public sealed class InMemoryGrievanceScoreRepository : IGrievanceScoreRepository
{
    private readonly ConcurrentDictionary<Guid, GrievanceScore> _store = new();

    public Task<IEnumerable<GrievanceScore>> GetByProvinceAndGrievanceAsync(
        Province province, GrievanceCategory category, int month, int year, CancellationToken ct = default)
    {
        var results = _store.Values.Where(s =>
            s.Province.Value          == province.Value &&
            s.GrievanceCategory.Value == category.Value &&
            s.ScoreMonth              == month          &&
            s.ScoreYear               == year);

        return Task.FromResult<IEnumerable<GrievanceScore>>(results.ToList());
    }

    public Task UpsertAsync(GrievanceScore score, CancellationToken ct = default)
    {
        _store[score.Id] = score;
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;
}
