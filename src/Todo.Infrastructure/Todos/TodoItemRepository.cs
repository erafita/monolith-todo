namespace Todo.Infrastructure.Todos;

internal sealed class TodoItemRepository(ApplicationDbContext context) : ITodoItemRepository
{
    public async Task<List<TodoItem>> GetAllAsync(UserId userId, CancellationToken cancellationToken = default) =>
        await context.TodoItems
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .ToListAsync(cancellationToken);

    public async Task<TodoItem?> GetByIdAsync(TodoItemId id, CancellationToken cancellationToken = default) =>
        await context.TodoItems
            .AsNoTracking()
            .FirstOrDefaultAsync(
                t => t.Id == id.Value, cancellationToken);

    public void Add(TodoItem entity)
    {
        context.Add(entity);
    }

    public void Update(TodoItem entity)
    {
        context.Update(entity);
    }
}
