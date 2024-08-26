namespace BuildingBlocks;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}