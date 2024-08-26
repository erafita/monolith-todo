using Todo.Application.Users.GetById;

namespace Todo.Application.Tests.Users;

public class GetUserByIdTests
{
    private readonly GetUserByIdQueryHandler handler;
    private readonly GetUserByIdQueryValidator validator;
    private readonly IUserRepository repository = Substitute.For<IUserRepository>();

    public GetUserByIdTests()
    {
        validator = new GetUserByIdQueryValidator();
        handler = new GetUserByIdQueryHandler(repository);
    }

    [Fact]
    public async Task GetUser_ShouldFail_WhenUserDoesNotExists()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .Build();

        var query = new GetUserByIdQuery(user.Id);

        repository
            .GetByIdAsync(
                Arg.Any<UserId>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        Result<UserResponse> result =
            await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.NotFound(user.Id));

        await repository.Received(1)
            .GetByIdAsync(
                Arg.Any<UserId>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUserById_ShouldSuccess_WhenRequestIsValid()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .Build();

        var query = new GetUserByIdQuery(user.Id);

        repository
            .GetByIdAsync(
                Arg.Any<UserId>(), Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        Result<UserResponse> result =
            await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        await repository.Received(1)
            .GetByIdAsync(
                Arg.Any<UserId>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public void GetUserByIdValidator_ShouldHaveError_WhenRequestIsEmpty()
    {
        // Arrange
        var query = new GetUserByIdQuery(Guid.Empty);

        // Act
        TestValidationResult<GetUserByIdQuery> result = validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.UserId);
    }
}
