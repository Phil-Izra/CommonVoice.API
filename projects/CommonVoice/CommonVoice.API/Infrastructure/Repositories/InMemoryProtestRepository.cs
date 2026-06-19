namespace CommonVoice.API.Infrastructure.Repositories;

using System.Collections.Concurrent;
using CommonVoice.API.Domain.Aggregates.Protests;
using CommonVoice.API.Domain.Repositories;
using CommonVoice.API.Domain.ValueObjects;

public sealed class InMemoryProtestRepository : IProtestRepository
{
    private readonly ConcurrentDictionary<Guid, Protest> _store = new();

    public Task<Protest?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(_store.TryGetValue(id, out var p) ? p : null);

    public Task<Protest?> GetByJoinTokenAsync(string token, CancellationToken ct = default) =>
        Task.FromResult(_store.Values.FirstOrDefault(p => p.JoinToken == token));

    public Task<IEnumerable<Protest>> GetByProvinceAndGrievanceAsync(
        Province province, GrievanceCategory category, int month, int year, CancellationToken ct = default)
    {
        var results = _store.Values.Where(p =>
            p.Province.Value          == province.Value  &&
            p.GrievanceCategory.Value == category.Value  &&
            p.Status                  == ProtestStatus.Completed &&
            p.EndedAt.HasValue        &&
            p.EndedAt.Value.Month     == month           &&
            p.EndedAt.Value.Year      == year);

        return Task.FromResult<IEnumerable<Protest>>(results.ToList());
    }

    public Task AddAsync(Protest protest, CancellationToken ct = default)
    {
        _store[protest.Id] = protest;
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;
}
