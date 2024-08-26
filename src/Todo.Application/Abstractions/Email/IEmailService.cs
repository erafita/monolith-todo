namespace Todo.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendEmailConfirmationAsync(string emailAddress);
}
