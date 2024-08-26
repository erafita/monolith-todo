namespace Todo.Domain.Todos;

public interface ITodoItemRepository : IRepository<TodoItem>
{
    Task<List<TodoItem>> GetAllAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<TodoItem?> GetByIdAsync(TodoItemId id, CancellationToken cancellationToken = default);
}
