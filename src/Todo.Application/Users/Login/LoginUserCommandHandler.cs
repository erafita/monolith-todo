namespace Todo.Application.Users.Login;

internal sealed class LoginUserCommandHandler(
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider,
    IUserRepository repository)
    : ICommandHandler<LoginUserCommand, string>
{
    public async Task<Result<string>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        Result<Email> email = Email.Create(command.Email);

        if (email.IsFailure)
        {
            return Result.Failure<string>(email.Error);
        }

        User? user = await repository.GetByEmailAsync(email.Value, cancellationToken);

        if (user is null)
        {
            return Result.Failure<string>(UserErrors.InvalidEmailOrPassword);
        }

        bool verified = passwordHasher.Verify(command.Password, user.Password);

        if (!verified)
        {
            return Result.Failure<string>(UserErrors.InvalidEmailOrPassword);
        }

        string token = tokenProvider.Create(user);

        return token;
    }
}
