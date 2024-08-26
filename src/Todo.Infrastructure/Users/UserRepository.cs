namespace Todo.Infrastructure.Users;

internal sealed class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<bool> EmailAlreadyExistsAsync(Email email, CancellationToken cancellationToken = default) =>
        await context.Users
            .AnyAsync(
                u => u.Email.Value == email, cancellationToken);

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default) =>
        await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.Email.Value == email, cancellationToken);

    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default) =>
        await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.Id == id.Value, cancellationToken);

    public void Add(User entity)
    {
        context.Add(entity);
    }

    public void Update(User entity)
    {
        context.Update(entity);
    }
}
