namespace Todo.Infrastructure.Persistence.Interceptors;

public class AuditableEntitiesInterceptor(IDateTimeProvider dateTimeProvider) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateAuditableEntities(eventData.Context, dateTimeProvider);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateAuditableEntities(DbContext context, IDateTimeProvider dateTimeProvider)
    {
        DateTime utcNow = dateTimeProvider.UtcNow;

        var entities = context.ChangeTracker.Entries<IAuditableEntity>().ToList();

        foreach (EntityEntry<IAuditableEntity>? entry in entities)
        {
            if (entry.State == EntityState.Added)
            {
                SetCurrentPropertyValue(entry, nameof(IAuditableEntity.CreatedAt), utcNow);
            }

            if (entry.State == EntityState.Modified)
            {
                SetCurrentPropertyValue(entry, nameof(IAuditableEntity.UpdatedAt), utcNow);
            }
        }

        static void SetCurrentPropertyValue(EntityEntry entry, string propertyName, DateTime utcNow)
            => entry.Property(propertyName).CurrentValue = utcNow;
    }
}
