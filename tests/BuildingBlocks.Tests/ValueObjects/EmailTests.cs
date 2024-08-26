namespace BuildingBlocks.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void CreateEmail_ShouldFail_WhenValueIsEmpty()
    {
        // Act
        Result<Email> email = Email.Create(string.Empty);

        // Assert
        email.IsFailure.Should().BeTrue();
        email.Error.Should().Be(EmailErrors.NullOrEmpty);
    }

    [Fact]
    public void CreateEmail_ShouldFail_WhenValueIsNotValid()
    {
        // Act
        Result<Email> email = Email.Create("emailname@email");

        // Assert
        email.IsFailure.Should().BeTrue();
        email.Error.Should().Be(EmailErrors.InvalidFormat);
    }

    [Fact]
    public void CreateEmail_ShouldFail_WhenValueIsLongerThanAllowed()
    {
        // Act
        Result<Email> email = Email.Create($"{new string('a', Email.MaxLength)}@email.com");

        // Assert
        email.IsFailure.Should().BeTrue();
        email.Error.Should().Be(EmailErrors.LongerThanAllowed);
    }

    [Fact]
    public void CreateEmail_ShouldSuccess_WhenValueIsValid()
    {
        // Arrange
        string value = "username@email.com";

        // Act
        Result<Email> email = Email.Create(value);

        // Assert
        email.IsSuccess.Should().BeTrue();
        email.Value.Value.Should().Be(value);
    }
}
