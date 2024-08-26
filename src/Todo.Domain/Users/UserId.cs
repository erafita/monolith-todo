namespace Todo.Domain.Users;

public class UserId(Guid value)
{
    public Guid Value = Guard.NotEmpty(value);

    public static implicit operator Guid(UserId id) =>
        id.Value;
}
