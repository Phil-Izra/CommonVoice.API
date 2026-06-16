namespace CommonVoice.API.Application.Events.Handlers;

using CommonVoice.API.Domain.Aggregates.Users;
using Microsoft.Extensions.Logging;

public sealed class UserRegisteredEventHandler(ILogger<UserRegisteredEventHandler> logger)
    : IDomainEventHandler<UserRegisteredEvent>
{
    public Task HandleAsync(UserRegisteredEvent e, CancellationToken ct = default)
    {
        logger.LogInformation(
            "User registered — Id: {UserId}, Email: {Email}, Role: {Role}",
            e.UserId, e.Email, e.Role);
        return Task.CompletedTask;
    }
}
