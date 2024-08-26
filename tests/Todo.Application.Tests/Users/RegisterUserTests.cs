namespace Todo.Application.Tests.Users;

public class RegisterUserTests
{
    private readonly RegisterUserCommandHandler handler;
    private readonly RegisterUserCommandValidator validator;
    private readonly IUnitOfWork uow = Substitute.For<IUnitOfWork>();
    private readonly IUserRepository repository = Substitute.For<IUserRepository>();
    private readonly IPasswordHasher passwordHasher = Substitute.For<IPasswordHasher>();

    public RegisterUserTests()
    {
        validator = new RegisterUserCommandValidator();
        handler = new RegisterUserCommandHandler(passwordHasher, repository, uow);
    }

    [Fact]
    public async Task RegisterUser_ShouldFail_WhenEmailAlreadyExists()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .WithLastName("lastname")
            .WithFirstName("firstname")
            .WithPassword("Pa$sword123!")
            .WithEmail("username@domain.com")
            .Build();

        var command = new RegisterUserCommand(
            user.Email,
            user.FirstName,
            user.LastName,
            user.Password);

        repository
            .EmailAlreadyExistsAsync(
                Arg.Any<Email>(), Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        Result<Guid> result =
            await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.EmailNotUnique);

        await repository.Received(1)
            .EmailAlreadyExistsAsync(
                Arg.Any<Email>(), Arg.Any<CancellationToken>());

        passwordHasher.Received(0)
            .Hash(Arg.Any<string>());

        repository.Received(0)
            .Add(Arg.Any<User>());

        await uow.Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RegisterUser_ShouldSuccess_WhenRequestIsValid()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .WithLastName("lastname")
            .WithFirstName("firstname")
            .WithPassword("Pa$sword123!")
            .WithEmail("username@domain.com")
            .Build();

        var command = new RegisterUserCommand(
            user.Email,
            user.FirstName,
            user.LastName,
            user.Password);

        repository
            .EmailAlreadyExistsAsync(
                Arg.Any<Email>(), Arg.Any<CancellationToken>())
            .Returns(false);

        passwordHasher
            .Hash(Arg.Any<string>())
            .Returns("Pa$sword123!");

        // Act
        Result<Guid> result =
            await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        await repository.Received(1)
            .EmailAlreadyExistsAsync(
                Arg.Any<Email>(), Arg.Any<CancellationToken>());

        passwordHasher.Received(1)
            .Hash(Arg.Any<string>());

        repository.Received(1)
            .Add(Arg.Any<User>());

        await uow.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void RegisterUserValidator_ShouldHaveError_WhenRequestIsEmpty()
    {
        // Arrange
        var command = new RegisterUserCommand(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty);

        // Act
        TestValidationResult<RegisterUserCommand> result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Email);
        result.ShouldHaveValidationErrorFor(command => command.Password);
        result.ShouldHaveValidationErrorFor(command => command.FirstName);
    }
}
