namespace Todo.Application.Tests.Users;

public class LoginUserTests
{
    private readonly LoginUserCommandHandler handler;
    private readonly LoginUserCommandValidator validator;
    private readonly IUserRepository repository = Substitute.For<IUserRepository>();
    private readonly ITokenProvider tokenProvider = Substitute.For<ITokenProvider>();
    private readonly IPasswordHasher passwordHasher = Substitute.For<IPasswordHasher>();

    public LoginUserTests()
    {
        validator = new LoginUserCommandValidator();
        handler = new LoginUserCommandHandler(passwordHasher, tokenProvider, repository);
    }

    [Fact]
    public async Task LoginUser_ShouldFail_WhenEmailDoesNotExists()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .Build();

        var command = new LoginUserCommand(user.Email, user.Password);

        repository
            .GetByEmailAsync(
                Arg.Any<Email>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        Result<string> result =
            await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.InvalidEmailOrPassword);

        await repository.Received(1)
            .GetByEmailAsync(
                Arg.Any<Email>(), Arg.Any<CancellationToken>());

        passwordHasher.Received(0)
            .Verify(Arg.Any<string>(), Arg.Any<string>());

        tokenProvider.Received(0)
            .Create(Arg.Any<User>());
    }

    [Fact]
    public async Task LoginUser_ShouldFail_WhenPasswordIsNotValid()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .Build();

        var command = new LoginUserCommand(user.Email, user.Password);

        repository
            .GetByEmailAsync(
                Arg.Any<Email>(), Arg.Any<CancellationToken>())
            .Returns(user);

        passwordHasher
            .Verify(Arg.Any<string>(), Arg.Any<string>())
            .Returns(false);

        // Act
        Result<string> result =
            await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.InvalidEmailOrPassword);

        await repository.Received(1)
            .GetByEmailAsync(
                Arg.Any<Email>(), Arg.Any<CancellationToken>());

        passwordHasher.Received(1)
            .Verify(Arg.Any<string>(), Arg.Any<string>());

        tokenProvider.Received(0)
            .Create(Arg.Any<User>());
    }

    [Fact]
    public async Task LoginUser_ShouldSuccess_WhenRequestIsValid()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .Build();

        var command = new LoginUserCommand(user.Email, user.Password);

        repository
            .GetByEmailAsync(
                Arg.Any<Email>(), Arg.Any<CancellationToken>())
            .Returns(user);

        passwordHasher
            .Verify(Arg.Any<string>(), Arg.Any<string>())
            .Returns(true);

        tokenProvider
            .Create(Arg.Any<User>())
            .Returns("NewToken1234567890");

        // Act
        Result<string> result =
            await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("NewToken1234567890");

        await repository.Received(1)
            .GetByEmailAsync(
                Arg.Any<Email>(), Arg.Any<CancellationToken>());

        passwordHasher.Received(1)
            .Verify(Arg.Any<string>(), Arg.Any<string>());

        tokenProvider.Received(1)
            .Create(Arg.Any<User>());
    }

    [Fact]
    public void LoginUserValidator_ShouldHaveError_WhenRequestIsEmpty()
    {
        // Arrange
        var command = new LoginUserCommand(string.Empty, string.Empty);

        // Act
        TestValidationResult<LoginUserCommand> result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Email);
        result.ShouldHaveValidationErrorFor(command => command.Password);
    }
}
