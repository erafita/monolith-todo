namespace BuildingBlocks;

public interface IRepository<TEntity>
{
    void Add(TEntity entity);

    void Update(TEntity entity);
}