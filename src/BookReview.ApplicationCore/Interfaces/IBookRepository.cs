using BookReview.ApplicationCore.Domain;

namespace BookReview.ApplicationCore.Interfaces;

public interface IBookRepository
{
    Task<IList<Book>> GetBooksByFilterAsync(BookFilter filter);
}
