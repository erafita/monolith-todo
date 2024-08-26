namespace Todo.Infrastructure.Emails;

internal sealed class EmailService : IEmailService
{
    public Task SendEmailConfirmationAsync(string emailAddress)
    {
        return Task.CompletedTask;
    }
}
