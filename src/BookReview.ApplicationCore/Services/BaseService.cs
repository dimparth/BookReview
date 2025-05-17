using BookReview.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookReview.ApplicationCore.Services;

public abstract class BaseService<TEntity, TId> : IService<TEntity, TId>
    where TEntity : class
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly ILogger _logger;

    protected BaseService(ILogger logger,  IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    protected abstract IRepository<TEntity, TId> GetRepository();
    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        _logger.LogInformation("Adding entity of type {EntityType}", typeof(TEntity).Name);
        var newEntity = await GetRepository().AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return newEntity;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        _logger.LogInformation("Getting all entities of type {EntityType}", typeof(TEntity).Name);
        return await GetRepository().GetAllAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        _logger.LogInformation("Getting entity of type {EntityType} with id {Id}", typeof(TEntity).Name, id);
        return await GetRepository().GetByIdAsync(id);
    }

    public async Task<bool> RemoveAsync(TEntity entity)
    {
        _logger.LogInformation("Removing entity of type {EntityType}", typeof(TEntity).Name);
        var deleted = await GetRepository().DeleteAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return deleted;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _logger.LogInformation("Updating entity of type {EntityType}", typeof(TEntity).Name);
        var updated = await GetRepository().UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return updated;
    }
}
