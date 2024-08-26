namespace Todo.Application.Todos.Create;

public sealed record CreateTodoCommand(
    Guid UserId, string Description, DateTime? DueDate = null, Priority? Priority = null, List<string>? Labels = null) : ICommand<Guid>;
