namespace CommonVoice.API.Domain.Aggregates.Protests;

using CommonVoice.API.Domain.Shared;

public record ProtestCreatedEvent(Guid ProtestId, string Province, string GrievanceCategory)
    : IDomainEvent { public DateTime OccurredAt => DateTime.UtcNow; }

public record ProtestActivatedEvent(Guid ProtestId, string JoinToken)
    : IDomainEvent { public DateTime OccurredAt => DateTime.UtcNow; }

public record ProtestCompletedEvent(Guid ProtestId, string Province, string GrievanceCategory)
    : IDomainEvent { public DateTime OccurredAt => DateTime.UtcNow; }
