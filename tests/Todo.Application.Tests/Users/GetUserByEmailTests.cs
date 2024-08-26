using Todo.Application.Users.GetByEmail;

namespace Todo.Application.Tests.Users;

public class GetUserByEmailTests
{
    private readonly GetUserByEmailQueryHandler handler;
    private readonly GetUserByEmailQueryValidator validator;
    private readonly IUserRepository repository = Substitute.For<IUserRepository>();

    public GetUserByEmailTests()
    {
        validator = new GetUserByEmailQueryValidator();
        handler = new GetUserByEmailQueryHandler(repository);
    }

    [Fact]
    public async Task GetUserByEmail_ShouldFail_WhenUserDoesNotExists()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .Build();

        var query = new GetUserByEmailQuery(user.Email);

        repository
            .GetByEmailAsync(
                Arg.Any<Email>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        Result<UserResponse> result =
            await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.NotFoundByEmail);

        await repository.Received(1)
            .GetByEmailAsync(
                Arg.Any<Email>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUserByEmail_ShouldSuccess_WhenRequestIsValid()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .Build();

        var query = new GetUserByEmailQuery(user.Email);

        repository
            .GetByEmailAsync(
                Arg.Any<Email>(), Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        Result<UserResponse> result =
            await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Value.Should().Be(
            new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                LastName = user.LastName,
                FirstName = user.FirstName
            });

        result.IsSuccess.Should().BeTrue();

        await repository.Received(1)
            .GetByEmailAsync(
                Arg.Any<Email>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public void GetUserByEmailValidator_ShouldHaveError_WhenRequestIsEmpty()
    {
        // Arrange
        var query = new GetUserByEmailQuery(string.Empty);

        // Act
        TestValidationResult<GetUserByEmailQuery> result = validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Email);
    }
}
