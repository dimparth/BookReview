using BookReview.ApplicationCore.Domain;
using BookReview.ApplicationCore.Interfaces;
using BookReview.Web.Extensions;
using BookReview.Web.Models.Resources;
using Microsoft.AspNetCore.Mvc;

namespace BookReview.Web.Controllers.Api;

[Route("api/books")]
[ApiController]
public sealed class BookControllerApi : ControllerBase
{

    private readonly IBookService _bookService;
    private readonly IReviewService _reviewService;
    public BookControllerApi(IBookService bookService, IReviewService reviewService)
    {
        _bookService = bookService;
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<ActionResult<IList<BookResource>>> GetBooksAsync([FromQuery] BookFilterResource filter)
    {
        var books = (filter.IsEmpty()) ? await _bookService.GetAllAsync() :
            await _bookService.GetBooksByFiltersAsync(filter.ToDomain());
        var resource = books.Select(b => b.ToResource()).ToList();
        return Ok(resource);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookResource>> GetBookByIdAsync(long id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book is null)
        {
            return NotFound();
        }
        return Ok(book.ToResource());
    }

    [HttpGet("{id}/reviews")]
    public async Task<ActionResult<IList<ReviewResource>>> GetBookReviewsAsync(long id)
    {
        var reviews = await _reviewService.GetByBookIdAsync(id);
        if (reviews is null)
        {
            return NotFound();
        }
        var resource = reviews.Select(r => r.ToResource()).ToList();
        return Ok(resource);
    }

    [HttpPost]
    public async Task<ActionResult<BookResource>> CreateBookAsync([FromBody] CreateBookResource book)
    {
        if (book is null)
        {
            return BadRequest("Book cannot be null");
        }
        var domain = Book.Create(book.Title, book.Author, book.PublishedYear, book.Genre);
        var createdBook = await _bookService.AddAsync(domain);
        return CreatedAtAction(nameof(CreateBookAsync), new { id = createdBook.Id }, createdBook);
    }
}
