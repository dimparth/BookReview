using BookReview.ApplicationCore.Domain;
using BookReview.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookReview.Infrastructure.Data.Repositories;

public sealed class BookRepository : BaseRepository<Book, long>, IBookRepository
{
    public BookRepository(BookReviewContext context) : base(context)
    {
    }

    public async Task<Book?> GetBookAndReviewsByIdAsync(long bookId)
    {
        return await _dbSet
            .Include(b => b.Reviews)
            .ThenInclude(r => r.Votes)
            .Include(b => b.Reviews)
            .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(b => b.Id == bookId);
    }
    public async Task<IList<Book>> GetBooksByFilterAsync(BookFilter filter)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.TitleContains))
            query = query.Where(b => b.Title.Contains(filter.TitleContains));

        if (!string.IsNullOrWhiteSpace(filter.AuthorContains))
            query = query.Where(b => b.Author.Contains(filter.AuthorContains));

        if (filter.MinAverageRating.HasValue)
            query = query.Where(b => b.Reviews.Any()
                && b.Reviews.Average(r => r.Rating) >= filter.MinAverageRating.Value);

        if (filter.MaxAverageRating.HasValue)
            query = query.Where(b => b.Reviews.Any()
                && b.Reviews.Average(r => r.Rating) <= filter.MaxAverageRating.Value);

        if (!string.IsNullOrWhiteSpace(filter.Genre))
            query = query.Where(b => b.Genre.Contains(filter.Genre));

        if (!string.IsNullOrWhiteSpace(filter.SortBy))
        {
            query = filter.SortBy.ToLower() switch
            {
                "title" => filter.Descending ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title),
                "author" => filter.Descending ? query.OrderByDescending(b => b.Author) : query.OrderBy(b => b.Author),
                "year" => filter.Descending ? query.OrderByDescending(b => b.PublishedYear) : query.OrderBy(b => b.PublishedYear),
                "rating" => filter.Descending
                    ? query.OrderByDescending(b => b.Reviews.Any() ? b.Reviews.Average(r => r.Rating) : 0)
                    : query.OrderBy(b => b.Reviews.Any() ? b.Reviews.Average(r => r.Rating) : 0),
                _ => query
            };
        }

        if (filter.Skip.HasValue)
            query = query.Skip(filter.Skip.Value);

        if (filter.Take.HasValue)
            query = query.Take(filter.Take.Value);

        return await query
    .Include(b => b.Reviews)
        .ThenInclude(r => r.User)
    .Include(b => b.Reviews)
        .ThenInclude(r => r.Votes)
    .ToListAsync();
    }
}