using BookReview.ApplicationCore.Domain;

namespace BookReview.ApplicationCore.Interfaces;

public interface IReviewRepository : IRepository<Review, long>
{
    Task<IList<Review>> GetReviewsForBookAsync(long bookId);
    Task<Review?> GetReviewWithVotesByIdAsync(long reviewId);
    Task<bool> VoteAsync(long reviewId, long userId, bool isUpvote);
}