namespace Todo.Application.Todos.Complete;

internal sealed class CompleteTodoCommandHandler(
    ITodoItemRepository repository,
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CompleteTodoCommand>
{
    public async Task<Result> Handle(CompleteTodoCommand command, CancellationToken cancellationToken)
    {
        TodoItem? todoItem = await repository
            .GetByIdAsync(new TodoItemId(command.TodoItemId), cancellationToken);

        if (todoItem == null)
        {
            return Result.Failure(TodoItemErrors.NotFound(command.TodoItemId));
        }

        todoItem.SetCompleted(true, dateTimeProvider.UtcNow);

        repository.Update(todoItem);

        await uow.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
