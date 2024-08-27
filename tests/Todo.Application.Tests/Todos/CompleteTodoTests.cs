namespace Todo.Application.Tests.Todos;

public class CompleteTodoTests
{
    private readonly CompleteTodoCommandHandler handler;
    private readonly CompleteTodoCommandValidator validator;
    private readonly IUnitOfWork uow = Substitute.For<IUnitOfWork>();
    private readonly IDateTimeProvider timeProvider = Substitute.For<IDateTimeProvider>();
    private readonly ITodoItemRepository repository = Substitute.For<ITodoItemRepository>();

    public CompleteTodoTests()
    {
        validator = new CompleteTodoCommandValidator();
        handler = new CompleteTodoCommandHandler(repository, uow, timeProvider);
    }

    [Fact]
    public async Task CompleteTodo_ShouldFail_WhenTodoDoesNotExists()
    {
        // Arrange
        TodoItem todo = TodoItemBuilder
            .Empty
            .Build();

        var command = new CompleteTodoCommand(todo.Id);

        repository
            .GetByIdAsync(
                Arg.Any<TodoItemId>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        Result result =
            await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoItemErrors.NotFound(todo.Id));

        await repository.Received(1)
            .GetByIdAsync(
                Arg.Any<TodoItemId>(), Arg.Any<CancellationToken>());

        repository.Received(0)
            .Update(Arg.Any<TodoItem>());

        await uow.Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CompleteTodo_ShouldSuccess_WhenRequestIsValid()
    {
        // Arrange
        TodoItem todo = TodoItemBuilder
            .Empty
            .Build();

        var command = new CompleteTodoCommand(todo.Id);

        repository
            .GetByIdAsync(
                Arg.Any<TodoItemId>(), Arg.Any<CancellationToken>())
            .Returns(todo);

        // Act
        Result result =
            await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        await repository.Received(1)
            .GetByIdAsync(
                Arg.Any<TodoItemId>(), Arg.Any<CancellationToken>());

        repository.Received(1)
            .Update(Arg.Any<TodoItem>());

        await uow.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void CompleteTodoValidator_ShouldHaveError_WhenRequestIsEmpty()
    {
        // Arrange
        var command = new CompleteTodoCommand(Guid.Empty);

        // Act
        TestValidationResult<CompleteTodoCommand> result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.TodoItemId);
    }
}
