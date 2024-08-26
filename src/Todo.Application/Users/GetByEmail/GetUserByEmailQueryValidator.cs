namespace Todo.Application.Users.GetByEmail;

internal sealed class GetUserByEmailQueryValidator
    : AbstractValidator<GetUserByEmailQuery>
{
    public GetUserByEmailQueryValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);
    }
}
