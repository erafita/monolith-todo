namespace Todo.Application.Users.Register;

internal sealed class UserRegisteredDomainEventHandler(
    IUserRepository repository,
    IEmailService emailService)
    : INotificationHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        User? user = await repository.GetByIdAsync(new UserId(notification.UserId), cancellationToken)
            ?? throw new ApplicationException(UserErrors.NotFound(notification.UserId).Description);

        await emailService.SendEmailConfirmationAsync(user.Email.Value);
    }
}
