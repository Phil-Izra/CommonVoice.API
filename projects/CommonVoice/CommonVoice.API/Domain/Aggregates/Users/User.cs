namespace CommonVoice.API.Domain.Aggregates.Users;

using CommonVoice.API.Domain.Shared;
using CommonVoice.API.Domain.ValueObjects;

public class User : AggregateRoot
{
    public string    Name         { get; private set; } = default!;
    public string    Email        { get; private set; } = default!;
    public string    PasswordHash { get; private set; } = default!;
    public UserRole  Role         { get; private set; }
    public string?   Sector       { get; private set; }   // only for Role == User
    public Province? Province     { get; private set; }   // only for Role == User
    public bool      IsVerified   { get; private set; }
    public DateTime  CreatedAt    { get; private set; }

    private User() { }

    public static User Register(
        string name,
        string email,
        string passwordHash,
        UserRole role,
        string? sector = null,
        Province? province = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        if (role == UserRole.User && (sector is null || province is null))
            throw new ArgumentException("Organisers must supply a sector and province");

        var u = new User
        {
            Name         = name.Trim(),
            Email        = email.Trim().ToLower(),
            PasswordHash = passwordHash,
            Role         = role,
            Sector       = sector?.Trim(),
            Province     = province,
            IsVerified   = false,
            CreatedAt    = DateTime.UtcNow,
        };

        u.Raise(new UserRegisteredEvent(u.Id, u.Email, u.Role));
        return u;
    }

    public void Verify() => IsVerified = true;

    public void UpdatePasswordHash(string newHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newHash);
        PasswordHash = newHash;
    }
}

public record UserRegisteredEvent(Guid UserId, string Email, UserRole Role)
    : IDomainEvent
{
    public DateTime OccurredAt => DateTime.UtcNow;
}
