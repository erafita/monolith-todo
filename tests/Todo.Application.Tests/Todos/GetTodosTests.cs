using Todo.Application.Todos.Get;

namespace Todo.Application.Tests.Todos;

public class GetTodosTests
{
    private readonly GetTodosQueryHandler handler;
    private readonly GetTodosQueryValidator validator;
    private readonly ITodoItemRepository repository = Substitute.For<ITodoItemRepository>();

    public GetTodosTests()
    {
        validator = new GetTodosQueryValidator();
        handler = new GetTodosQueryHandler(repository);
    }

    [Fact]
    public async Task GetTodoItems_ShouldSuccess_WhenRequestIsValid()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .Build();

        var todos = Enumerable
            .Range(1, 10)
            .Select(number => TodoItemBuilder.Empty.Build())
            .ToList();

        var query = new GetTodosQuery(user.Id);

        repository
            .GetAllAsync(
                Arg.Any<UserId>(), Arg.Any<CancellationToken>())
            .Returns(todos);

        // Act
        Result<List<TodoResponse>> result =
            await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetTodoItems_ShouldSuccess_WhenResultIsEmpty()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .Build();

        var query = new GetTodosQuery(user.Id);

        repository
            .GetAllAsync(
                Arg.Any<UserId>(), Arg.Any<CancellationToken>())
            .Returns([]);

        // Act
        Result<List<TodoResponse>> result =
            await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Value.Should().BeEmpty();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void GetTodoItemsValidator_ShouldHaveError_WhenRequestIsEmpty()
    {
        // Arrange
        var query = new GetTodosQuery(Guid.Empty);

        // Act
        TestValidationResult<GetTodosQuery> result = validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.UserId);
    }
}
