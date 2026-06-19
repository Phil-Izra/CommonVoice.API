namespace CommonVoice.API.Application.UseCases.Users;

using CommonVoice.API.Application.Common;
using CommonVoice.API.Application.Events;
using CommonVoice.API.Domain.Aggregates.Users;
using CommonVoice.API.Domain.Repositories;
using CommonVoice.API.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

public sealed record RegisterUserCommand(
    string   Name,
    string   Email,
    string   Password,
    UserRole Role,
    string?  Sector,
    string?  Province);

public sealed record RegisterUserResult(Guid UserId, string Email);

public sealed class RegisterUserUseCase(
    IUserRepository        users,
    IPasswordHasher        passwordHasher,
    IDomainEventDispatcher events,
    ILogger<RegisterUserUseCase> logger)
{
    public async Task<RegisterUserResult> ExecuteAsync(RegisterUserCommand cmd, CancellationToken ct = default)
    {
        logger.LogDebug("Registering user {Email}", cmd.Email);

        if (await users.GetByEmailAsync(cmd.Email, ct) is not null)
            throw new InvalidOperationException($"Email '{cmd.Email}' is already registered");

        var province = cmd.Province is not null ? new Province(cmd.Province) : null;

        var user = User.Register(
            cmd.Name,
            cmd.Email,
            passwordHasher.Hash(cmd.Password),
            cmd.Role,
            cmd.Sector,
            province);

        await users.AddAsync(user, ct);
        await users.SaveChangesAsync(ct);
        await events.DispatchAndClearAsync(user, ct);

        logger.LogInformation("User registered — {UserId}", user.Id);
        return new RegisterUserResult(user.Id, user.Email);
    }
}
