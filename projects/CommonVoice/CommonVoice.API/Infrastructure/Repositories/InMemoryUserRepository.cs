namespace CommonVoice.API.Infrastructure.Repositories;

using System.Collections.Concurrent;
using CommonVoice.API.Domain.Aggregates.Users;
using CommonVoice.API.Domain.Repositories;

public sealed class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<Guid, User> _store = new();

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(_store.TryGetValue(id, out var user) ? user : null);

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        Task.FromResult(_store.Values.FirstOrDefault(u => u.Email == email.Trim().ToLower()));

    public Task AddAsync(User user, CancellationToken ct = default)
    {
        _store[user.Id] = user;
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;
}
