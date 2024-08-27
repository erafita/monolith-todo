namespace Todo.Application.Tests.Todos;

public class CreateTodoTests
{
    private readonly CreateTodoCommandHandler handler;
    private readonly CreateTodoCommandValidator validator;
    private readonly IUnitOfWork uow = Substitute.For<IUnitOfWork>();
    private readonly IUserRepository userRepository = Substitute.For<IUserRepository>();
    private readonly ITodoItemRepository todoRepository = Substitute.For<ITodoItemRepository>();

    public CreateTodoTests()
    {
        validator = new CreateTodoCommandValidator();
        handler = new CreateTodoCommandHandler(userRepository, todoRepository, uow);
    }

    [Fact]
    public async Task CreateTodo_ShouldFail_WhenUserDoesNotExists()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .Build();

        TodoItem todo = TodoItemBuilder
            .Empty
            .Build();

        var command = new CreateTodoCommand(
            user.Id,
            todo.Description,
            todo.DueDate,
            todo.Priority,
            todo.Labels);

        userRepository
            .GetByIdAsync(
                Arg.Any<UserId>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        Result<Guid> result =
            await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.NotFound(user.Id));

        await userRepository.Received(1)
            .GetByIdAsync(
                Arg.Any<UserId>(), Arg.Any<CancellationToken>());

        todoRepository.Received(0)
            .Add(Arg.Any<TodoItem>());

        await uow.Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateTodo_ShouldSuccess_WhenRequestIsValid()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .Build();

        TodoItem todo = TodoItemBuilder
            .Empty
            .Build();

        var command = new CreateTodoCommand(
            user.Id,
            todo.Description,
            todo.DueDate,
            todo.Priority,
            todo.Labels);

        userRepository
            .GetByIdAsync(
                Arg.Any<UserId>(), Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        Result<Guid> result =
            await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        await userRepository.Received(1)
            .GetByIdAsync(
                Arg.Any<UserId>(), Arg.Any<CancellationToken>());

        todoRepository.Received(1)
            .Add(Arg.Any<TodoItem>());

        await uow.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void CreateTodoValidator_ShouldHaveError_WhenRequestIsEmpty()
    {
        // Arrange
        var command = new CreateTodoCommand(
            Guid.Empty,
            string.Empty);

        // Act
        TestValidationResult<CreateTodoCommand> result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.UserId);
        result.ShouldHaveValidationErrorFor(command => command.Description);
    }
}
