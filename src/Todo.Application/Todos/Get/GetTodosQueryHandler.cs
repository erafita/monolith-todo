namespace Todo.Application.Todos.Get;

internal sealed class GetTodosQueryHandler(ITodoItemRepository repository) : IQueryHandler<GetTodosQuery, List<TodoResponse>>
{
    public async Task<Result<List<TodoResponse>>> Handle(GetTodosQuery query, CancellationToken cancellationToken)
    {
        List<TodoItem> todoItems = await repository.GetAllAsync(new UserId(query.UserId), cancellationToken);

        return todoItems.Select(todoItem => new TodoResponse
        {
            Id = todoItem.Id,
            UserId = todoItem.UserId,
            Labels = todoItem.Labels,
            DueDate = todoItem.DueDate,
            CreatedAt = todoItem.CreatedAt,
            CompletedAt = todoItem.CompletedAt,
            Description = todoItem.Description,
            IsCompleted = todoItem.IsCompleted
        }).ToList();
    }
}
