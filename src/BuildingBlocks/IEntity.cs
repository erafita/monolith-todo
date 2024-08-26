namespace BuildingBlocks;

public interface IEntity
{
    IImmutableList<IDomainEvent> DomainEvents { get; }

    public void ClearDomainEvents();
}
