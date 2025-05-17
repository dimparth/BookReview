namespace BookReview.ApplicationCore.Interfaces;

public interface IService<TEntity, TId>
    where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(TId id);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<bool> RemoveAsync(TEntity entity);
}