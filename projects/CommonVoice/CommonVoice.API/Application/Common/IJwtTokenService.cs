namespace CommonVoice.API.Application.Common;

using CommonVoice.API.Domain.Aggregates.Users;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAt) Generate(User user);
}
