using BookReview.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookReview.Infrastructure.Data.Repositories;

public abstract class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
{
    protected readonly BookReviewContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(BookReviewContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
        => await _dbSet.FindAsync(id);

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        var entry = await _dbSet.AddAsync(entity);
        return entry.Entity;
    }

    public Task<TEntity> UpdateAsync(TEntity entity)
    {
        var entry = _dbSet.Update(entity);
        return Task.FromResult(entry.Entity);
    }

    public Task<bool> DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return Task.FromResult(true);
    }
}
