namespace CommonVoice.API.Models.Auth;

using CommonVoice.API.Domain.Aggregates.Users;

public sealed record AuthResponse(
    string   Token,
    DateTime ExpiresAt,
    Guid     UserId,
    string   Name,
    UserRole Role
);
