namespace CommonVoice.API.Domain.Repositories;

using CommonVoice.API.Domain.Aggregates.Protests;
using CommonVoice.API.Domain.ValueObjects;

public interface IProtestRepository
{
    Task<Protest?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Protest?> GetByJoinTokenAsync(string token, CancellationToken ct = default);
    Task<IEnumerable<Protest>> GetByProvinceAndGrievanceAsync(
        Province province, GrievanceCategory category,
        int month, int year, CancellationToken ct = default);
    Task AddAsync(Protest protest, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
