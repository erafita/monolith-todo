namespace Todo.Application.Todos.Get;

internal sealed class GetTodosQueryValidator : AbstractValidator<GetTodosQuery>
{
    public GetTodosQueryValidator()
    {
        RuleFor(t => t.UserId)
            .NotEmpty();
    }
}
