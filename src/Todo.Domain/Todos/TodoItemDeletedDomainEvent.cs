namespace Todo.Domain.Todos;

public sealed record TodoItemDeletedDomainEvent(Guid TodoItemId) : IDomainEvent;
