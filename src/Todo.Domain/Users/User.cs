namespace Todo.Domain.Users;

public class User : Entity<UserId>
{
    private User(Email email, string firstName, string? lastName, string password)
    {
        Email = email;
        LastName = lastName;
        Id = new UserId(Guid.NewGuid());
        Password = Guard.NotEmpty(password);
        FirstName = Guard.NotEmpty(firstName);
    }

    private User()
    { }

    public Email Email { get; private set; } = default!;
    public string FirstName { get; private set; } = default!;
    public string? LastName { get; private set; }
    public string Password { get; private set; } = default!;

    public static User Create(Email email, string firstName, string? lastName, string password) =>
        new User(email, firstName, lastName, password);

    public void UpdateName(string firstName, string? lastName = null)
    {
        LastName = lastName;
        FirstName = Guard.NotEmpty(firstName);
    }

    public void UpdateEmail(Email email)
    {
        Email = email;
    }

    public void UpdatePassword(string password)
    {
        Password = Guard.NotEmpty(password);
    }
}
