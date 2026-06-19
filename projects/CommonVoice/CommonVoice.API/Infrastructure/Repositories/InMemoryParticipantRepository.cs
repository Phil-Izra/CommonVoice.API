namespace CommonVoice.API.Infrastructure.Repositories;

using System.Collections.Concurrent;
using CommonVoice.API.Domain.Aggregates.Participants;
using CommonVoice.API.Domain.Repositories;

public sealed class InMemoryParticipantRepository : IParticipantRepository
{
    private readonly ConcurrentDictionary<Guid, Participant> _store = new();

    public Task<bool> ExistsAsync(Guid protestId, string deviceFingerprint, CancellationToken ct = default) =>
        Task.FromResult(_store.Values.Any(p =>
            p.ProtestId == protestId && p.DeviceFingerprint == deviceFingerprint));

    public Task<int> CountByProtestAsync(Guid protestId, CancellationToken ct = default) =>
        Task.FromResult(_store.Values.Count(p => p.ProtestId == protestId));

    public Task AddAsync(Participant participant, CancellationToken ct = default)
    {
        _store[participant.Id] = participant;
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;
}
