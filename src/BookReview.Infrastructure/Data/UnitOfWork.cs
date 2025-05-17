using BookReview.ApplicationCore.Interfaces;

namespace BookReview.Infrastructure.Data;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly BookReviewContext _context;
    public UnitOfWork(BookReviewContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
