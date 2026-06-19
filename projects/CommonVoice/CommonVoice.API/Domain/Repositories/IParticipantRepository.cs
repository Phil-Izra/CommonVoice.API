namespace CommonVoice.API.Domain.Repositories;

using CommonVoice.API.Domain.Aggregates.Participants;

public interface IParticipantRepository
{
    Task<bool> ExistsAsync(Guid protestId, string deviceFingerprint, CancellationToken ct = default);
    Task<int> CountByProtestAsync(Guid protestId, CancellationToken ct = default);
    Task AddAsync(Participant participant, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
