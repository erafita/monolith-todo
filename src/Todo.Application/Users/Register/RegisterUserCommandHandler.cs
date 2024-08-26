namespace Todo.Application.Users.Register;

internal sealed class RegisterUserCommandHandler(
    IPasswordHasher passwordHasher,
    IUserRepository repository,
    IUnitOfWork uow)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        Result<Email> email = Email.Create(command.Email);

        if (email.IsFailure)
        {
            return Result.Failure<Guid>(email.Error);
        }

        if (await repository.EmailAlreadyExistsAsync(email.Value, cancellationToken))
        {
            return Result.Failure<Guid>(UserErrors.EmailNotUnique);
        }

        var user = User.Create(
            email.Value,
            command.FirstName,
            command.LastName,
            passwordHasher.Hash(command.Password));

        user.AddDomainEvent(new UserRegisteredDomainEvent(user.Id));

        repository.Add(user);

        try
        {
            await uow.SaveChangesAsync(cancellationToken);

            return user.Id.Value;
        }
        catch (DbUpdateException e)
            when (e.InnerException is NpgsqlException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            return Result.Failure<Guid>(UserErrors.EmailNotUnique);
        }
    }
}
