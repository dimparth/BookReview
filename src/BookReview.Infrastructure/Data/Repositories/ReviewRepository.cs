using BookReview.ApplicationCore.Domain;
using BookReview.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookReview.Infrastructure.Data.Repositories;

public sealed class ReviewRepository : BaseRepository<Review, long>, IReviewRepository
{
    public ReviewRepository(BookReviewContext context) : base(context)
    {
    }

    public async Task<IList<Review>> GetReviewsForBookAsync(long bookId)
    {
        return await _dbSet
            .Where(r => r.BookId == bookId)
            .Include(r => r.Votes)
            .Include(r => r.User)
            .ToListAsync();
    }

    public async Task<bool> VoteAsync(long reviewId, long userId, bool isUpvote)
    {
        var review = await _dbSet
            .Include(r => r.Votes)
            .FirstOrDefaultAsync(r => r.Id == reviewId);

        if (review is null)
            return false;

        var existingVote = await _context.ReviewVotes
            .FirstOrDefaultAsync(v => v.ReviewId == reviewId && v.UserId == userId);

        if (existingVote is null)
        {
            var newVote = ReviewVote.Create(userId, reviewId, isUpvote);
            _context.ReviewVotes.Add(newVote);
        }
        else if (existingVote.IsUpvote == isUpvote)
        {
            _context.ReviewVotes.Remove(existingVote);
        }
        else
        {
            existingVote.Toggle();
            _context.ReviewVotes.Update(existingVote);
        }
        return true;
    }
}