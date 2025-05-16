using BookReview.ApplicationCore.Domain;
using BookReview.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.Infrastructure.Data.Repositories;

public sealed class BookRepository : BaseRepository<Book, long>, IBookRepository
{
    public BookRepository(BookReviewContext context) : base(context)
    {
    }

    public async Task<IList<Book>> GetBooksByFilterAsync(BookFilter filter)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.TitleContains))
            query = query.Where(b => b.Title.Contains(filter.TitleContains));

        if (!string.IsNullOrWhiteSpace(filter.AuthorContains))
            query = query.Where(b => b.Author.Contains(filter.AuthorContains));

        if (filter.MinAverageRating.HasValue)
            query = query.Where(b => b.Reviews.Any() &&
                b.Reviews.Average(r => r.Rating) >= filter.MinAverageRating.Value);

        if (filter.MaxAverageRating.HasValue)
            query = query.Where(b => b.Reviews.Any() &&
                b.Reviews.Average(r => r.Rating) <= filter.MaxAverageRating.Value);

        if (filter.Skip.HasValue)
            query = query.Skip(filter.Skip.Value);

        if (filter.Take.HasValue)
            query = query.Take(filter.Take.Value);

        return await query
            .Include(b => b.Reviews)
            .ToListAsync();
    }
}