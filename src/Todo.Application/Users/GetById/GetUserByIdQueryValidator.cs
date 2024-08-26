namespace Todo.Application.Users.GetById;

internal sealed class GetUserByIdQueryValidator
    : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(u => u.UserId)
            .NotEmpty();
    }
}
