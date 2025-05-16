using BookReview.ApplicationCore.Domain;

namespace BookReview.ApplicationCore.Interfaces;

public interface IReviewRepository : IRepository<Review, long>
{
    Task<IList<Review>> GetReviewsForBookAsync(long bookId);
}