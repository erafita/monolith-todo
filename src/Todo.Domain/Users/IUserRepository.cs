namespace Todo.Domain.Users;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

    Task<bool> EmailAlreadyExistsAsync(Email email, CancellationToken cancellationToken = default);
}
