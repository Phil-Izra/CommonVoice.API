namespace CommonVoice.API.Domain.Shared;

public interface IDomainEvent
{
    DateTime OccurredAt { get; }
}
