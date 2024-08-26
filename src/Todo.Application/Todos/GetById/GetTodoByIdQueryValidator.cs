namespace Todo.Application.Todos.GetById;

internal sealed class GetTodoByIdQueryValidator : AbstractValidator<GetTodoByIdQuery>
{
    public GetTodoByIdQueryValidator()
    {
        RuleFor(t => t.TodoItemId)
            .NotEmpty();
    }
}
