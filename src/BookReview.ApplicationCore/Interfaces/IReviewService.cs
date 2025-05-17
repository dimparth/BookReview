using BookReview.ApplicationCore.Domain;

namespace BookReview.ApplicationCore.Interfaces;

public interface IReviewService : IService<Review, long>
{
    Task<IEnumerable<Review>> GetByBookIdAsync(long bookId);
    Task<bool> VoteAsync(long reviewId, long userId, bool isUpvote);
}