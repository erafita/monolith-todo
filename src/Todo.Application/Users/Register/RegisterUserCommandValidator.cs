namespace Todo.Application.Users.Register;

internal sealed class RegisterUserCommandValidator
    : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(u => u.FirstName)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);

        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(16);

        RuleFor(u => u.LastName)
            .MaximumLength(255)
            .When(u => !string.IsNullOrEmpty(u.LastName));
    }
}
