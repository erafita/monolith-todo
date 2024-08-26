namespace Todo.Application.Todos.GetById;

internal sealed class GetTodoByIdQueryHandler(ITodoItemRepository repository) : IQueryHandler<GetTodoByIdQuery, TodoResponse>
{
    public async Task<Result<TodoResponse>> Handle(GetTodoByIdQuery query, CancellationToken cancellationToken)
    {
        TodoItem todoItem = await repository.GetByIdAsync(new TodoItemId(query.TodoItemId), cancellationToken);

        if (todoItem is null)
        {
            return Result.Failure<TodoResponse>(TodoItemErrors.NotFound(query.TodoItemId));
        }

        return new TodoResponse
        {
            Id = todoItem.Id,
            UserId = todoItem.UserId,
            Labels = todoItem.Labels,
            DueDate = todoItem.DueDate,
            CreatedAt = todoItem.CreatedAt,
            Description = todoItem.Description,
            IsCompleted = todoItem.IsCompleted,
            CompletedAt = todoItem.CompletedAt
        };
    }
}
