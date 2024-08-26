namespace Todo.Domain.Users;

public sealed record UserRegisteredDomainEvent(Guid UserId)
    : IDomainEvent;
