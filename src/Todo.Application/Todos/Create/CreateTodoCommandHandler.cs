namespace Todo.Application.Todos.Create;

internal sealed class CreateTodoCommandHandler(
    IUserRepository userRepository,
    ITodoItemRepository todoRepository,
    IUnitOfWork uow)
    : ICommandHandler<CreateTodoCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        var userId = new UserId(command.UserId);
        User? user = await userRepository.GetByIdAsync(new UserId(command.UserId), cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound(command.UserId));
        }

        var todoItem = TodoItem
            .Create(userId, command.Description, command.DueDate, command.Priority, command.Labels);

        todoItem.AddDomainEvent(new TodoItemCreatedDomainEvent(todoItem.Id));

        todoRepository.Add(todoItem);

        await uow.SaveChangesAsync(cancellationToken);

        return todoItem.Id.Value;
    }
}
