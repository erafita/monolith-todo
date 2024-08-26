namespace BuildingBlocks;

public abstract class Entity<TId> : IEntity
{
    private readonly List<IDomainEvent> domainEvents = [];

    protected Entity()
    { }

    public TId Id { get; init; } = default!;

    public IImmutableList<IDomainEvent> DomainEvents
        => [.. domainEvents];

    public void ClearDomainEvents()
        => domainEvents.Clear();

    public void AddDomainEvent(IDomainEvent domainEvent)
        => domainEvents.Add(domainEvent);
}
