using BookReview.ApplicationCore.Domain;

namespace BookReview.ApplicationCore.Interfaces;

public interface IBookService : IService<Book, long>
{
    Task<Book?> GetBookAndReviewsByIdAsync(long bookId);
    Task<IList<Book>> GetBooksByFiltersAsync(BookFilter filter);
}
