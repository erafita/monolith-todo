using Todo.Application.Todos.GetById;

namespace Todo.Application.Tests.Todos;

public class GetTodoByIdTests
{
    private readonly GetTodoByIdQueryHandler handler;
    private readonly GetTodoByIdQueryValidator validator;
    private readonly ITodoItemRepository repository = Substitute.For<ITodoItemRepository>();

    public GetTodoByIdTests()
    {
        validator = new GetTodoByIdQueryValidator();
        handler = new GetTodoByIdQueryHandler(repository);
    }

    [Fact]
    public async Task GetTodoItem_ShouldFail_WhenTodoItemDoesNotExists()
    {
        // Arrange
        TodoItem todo = TodoItemBuilder
            .Empty
            .Build();

        var query = new GetTodoByIdQuery(todo.Id);

        repository
            .GetByIdAsync(
                Arg.Any<TodoItemId>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        Result<TodoResponse> result =
            await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoItemErrors.NotFound(todo.Id));

        await repository.Received(1)
            .GetByIdAsync(
                Arg.Any<TodoItemId>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetTodoItem_ShouldSuccess_WhenRequestIsValid()
    {
        // Arrange
        TodoItem todo = TodoItemBuilder
            .Empty
            .Build();

        var query = new GetTodoByIdQuery(todo.Id);

        repository
            .GetByIdAsync(
                Arg.Any<TodoItemId>(), Arg.Any<CancellationToken>())
            .Returns(todo);

        // Act
        Result<TodoResponse> result =
            await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        await repository.Received(1)
            .GetByIdAsync(
                Arg.Any<TodoItemId>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public void GetTodoItemByIdValidator_ShouldHaveError_WhenRequestIsEmpty()
    {
        // Arrange
        var query = new GetTodoByIdQuery(Guid.Empty);

        // Act
        TestValidationResult<GetTodoByIdQuery> result = validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.TodoItemId);
    }
}
