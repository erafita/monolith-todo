namespace Todo.Domain.Tests;

public class UserTests
{
    private readonly Email email = Email.Create("username@domain.com").Value;

    [Fact]
    public void CreateUser_Should_ThrowException_WhenFirstNameIsEmpty()
    {
        // Act
        Func<User> fnc = () => User.Create(
            email,
            string.Empty,
            "LastName",
            "Pa$sword!");

        // Assert
        fnc.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreateUser_Should_ThrowException_WhenPasswordIsEmpty()
    {
        // Act
        Func<User> fnc = () => User.Create(
            email,
            "FirstName",
            "LastName",
            string.Empty);

        // Assert
        fnc.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreateUser_Should_Success()
    {
        // Act
        var user = User.Create(
            email,
            "FirstName",
            "LastName",
            "Pa$sword!");

        // Assert
        user.Id.Should().NotBeNull();
        user.Email.Should().Be(email);
        user.LastName.Should().Be("LastName");
        user.Password.Should().Be("Pa$sword!");
        user.FirstName.Should().Be("FirstName");
    }

    [Fact]
    public void UpdateUser_FirstName_Should_ThrowException_WhenFirstNameIsEmpty()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .Build();

        // Act
        Action act = () => user.UpdateName(string.Empty);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void UpdateUser_FirstName_Should_Success()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .Build();

        // Act
        user.UpdateName("NewUserName", "NewLastName");

        // Assert
        user.LastName.Should().Be("NewLastName");
        user.FirstName.Should().Be("NewUserName");
    }

    [Fact]
    public void UpdateUser_Password_Should_ThrowException_WhenPasswordIsEmpty()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .Build();

        // Act
        Action act = () => user.UpdatePassword(string.Empty);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void UpdateUser_Password_Should_Success()
    {
        // Arrange
        User user = UserBuilder
            .Empty
            .Build();

        // Act
        user.UpdatePassword("NewPa$sword!");

        // Assert
        user.Password.Should().Be("NewPa$sword!");
    }

    [Fact]
    public void UpdateUser_Email_Should_Success()
    {
        // Arrange
        Email email = Email
            .Create("newusername@domain.com")
            .Value;

        User user = UserBuilder
            .Empty
            .Build();

        // Act
        user.UpdateEmail(email);

        // Assert
        user.Email.Should().Be(email);
    }
}
