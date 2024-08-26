namespace Todo.Application.Users.Login;

internal sealed class LoginUserCommandValidator
    : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);

        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(16);
    }
}
