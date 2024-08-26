namespace Todo.Domain.Todos;

public class TodoItemId(Guid value)
{
    public Guid Value = Guard.NotEmpty(value);

    public static implicit operator Guid(TodoItemId id) =>
        id.Value;
}
