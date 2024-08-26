namespace Todo.Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IPublisher publisher) : DbContext(options), IUnitOfWork
{
    public const string Schema = "public";

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<TodoItem> TodoItems { get; set; } = default!;

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // BEFORE calling SaveChangesAsync
        //     - domain events are part of the same transaction
        //     - immediate consistency
        // AFTER calling SaveChangesAsync
        //     - domain events are a separate transaction
        //     - eventual consistency
        //     - handlers can fail

        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IImmutableList<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent);
        }
    }
}
