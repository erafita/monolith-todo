namespace Todo.Application.Todos.GetById;

public sealed record GetTodoByIdQuery(Guid TodoItemId) : IQuery<TodoResponse>;
