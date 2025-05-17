using BookReview.ApplicationCore.Domain;
using BookReview.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookReview.ApplicationCore.Services;

public sealed class BookService : BaseService<Book, long>, IBookService
{
    private readonly IBookRepository _bookRepository;
    public BookService(ILogger<BookService> logger, IBookRepository bookRepository, IUnitOfWork unitOfWork) : base(logger, unitOfWork)
    {
        _bookRepository = bookRepository;
    }

    protected override IRepository<Book, long> GetRepository()
    {
        return _bookRepository;
    }

    public async Task<Book?> GetBookAndReviewsByIdAsync(long bookId)
    {
        return await _bookRepository.GetBookAndReviewsByIdAsync(bookId);
    }
    public async Task<IList<Book>> GetBooksByFiltersAsync(BookFilter filter)
    {
        return await _bookRepository.GetBooksByFilterAsync(filter);
    }
}
