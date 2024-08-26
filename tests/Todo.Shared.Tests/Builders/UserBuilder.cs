namespace Todo.Shared.Tests.Builders;

public sealed class UserBuilder
{
    private string firstName = "N/A";
    private string lastName = "N/A";
    private string password = "N/A";
    private string email = "username@domain.com";

    private UserBuilder()
    { }

    public static UserBuilder Empty =>
        new();

    public UserBuilder WithFirstName(string firstName)
    {
        this.firstName = firstName;
        return this;
    }

    public UserBuilder WithLastName(string lastName)
    {
        this.lastName = lastName;
        return this;
    }

    public UserBuilder WithPassword(string password)
    {
        this.password = password;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        this.email = email;
        return this;
    }

    public User Build() =>
        User.Create(
            Email.Create(email).Value,
            firstName,
            lastName,
            password);
}
