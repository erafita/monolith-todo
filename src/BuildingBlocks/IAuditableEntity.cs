namespace BuildingBlocks;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; }

    DateTime? UpdatedAt { get; }
}
