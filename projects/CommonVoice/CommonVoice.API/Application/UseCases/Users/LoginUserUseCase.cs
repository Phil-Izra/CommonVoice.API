namespace CommonVoice.API.Application.UseCases.Users;

using CommonVoice.API.Application.Common;
using CommonVoice.API.Domain.Repositories;
using Microsoft.Extensions.Logging;

public sealed record LoginUserCommand(string Email, string Password);

public sealed record LoginUserResult(string Token, DateTime ExpiresAt, Guid UserId, string Name, string Role);

public sealed class LoginUserUseCase(
    IUserRepository  users,
    IPasswordHasher  passwordHasher,
    IJwtTokenService jwtTokenService,
    ILogger<LoginUserUseCase> logger)
{
    public async Task<LoginUserResult> ExecuteAsync(LoginUserCommand cmd, CancellationToken ct = default)
    {
        logger.LogDebug("Login attempt for {Email}", cmd.Email);

        var user = await users.GetByEmailAsync(cmd.Email, ct)
            ?? throw new UnauthorizedAccessException("Invalid credentials");

        if (!passwordHasher.Verify(cmd.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        var (token, expiresAt) = jwtTokenService.Generate(user);

        logger.LogInformation("User logged in — {UserId}", user.Id);
        return new LoginUserResult(token, expiresAt, user.Id, user.Name, user.Role.ToString());
    }
}
