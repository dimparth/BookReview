using BookReview.ApplicationCore.Domain;
using BookReview.ApplicationCore.Interfaces;
using BookReview.Web.Extensions;
using BookReview.Web.Models;
using BookReview.Web.Models.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReview.Web.Controllers;
[Authorize]
public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly IReviewService _reviewService;

    public BookController(IBookService bookService, IReviewService reviewService)
    {
        _bookService = bookService;
        _reviewService = reviewService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index([Bind(Prefix = "Filter")] BookFilterViewModel filterVm)
    {
        var filter = new BookFilter
        {
            Genre = filterVm.Genre,
            PublishedYear = filterVm.Year,
            MinAverageRating = filterVm.MinRating,
            MaxAverageRating = filterVm.MaxRating
        };

        var books = await _bookService.GetBooksByFiltersAsync(filter);

        var viewModel = new BookListViewModel
        {
            Books = books.Select(b => b.ToResource()).ToList(),
            Filter = new BookFilterViewModel
            {
                Genre = filterVm.Genre,
                Year = filterVm.Year,
                MinRating = filterVm.MinRating,
                MaxRating = filterVm.MaxRating
            }
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Reviews(long id)
    {
        var book = await _bookService.GetBookAndReviewsByIdAsync(id);
        if (book is null) return NotFound();

        var vm = new ReviewListViewModel
        {
            BookId = book.Id,
            BookTitle = book.Title,
            Reviews = book.Reviews.Select(r => r.ToResource()).ToList(),
            NewReview = User.Identity?.IsAuthenticated == true ? new ReviewCreateViewModel { BookId = book.Id } : null
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> AddReview(ReviewListViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var book = await _bookService.GetBookAndReviewsByIdAsync(model.BookId);
            if (book is null) return NotFound();

            var vm = new ReviewListViewModel
            {
                BookId = book.Id,
                BookTitle = book.Title,
                Reviews = book.Reviews.Select(r => r.ToResource()).ToList(),
                NewReview = model.NewReview
            };

            return View("Reviews", vm.BookId);
        }
        if (model.NewReview is null)
        {
            ModelState.AddModelError(string.Empty, "New review is required.");
            return View("Reviews", model.BookId);
        }
        var userId = User.GetUserId();
        var reviewDomain = Review.Create(model.NewReview.Content, model.NewReview.Rating, userId, model.NewReview.BookId);
        var result = await _reviewService.AddAsync(reviewDomain);

        if (result is null)
        {
            ModelState.AddModelError(string.Empty, "Error creating review");
            var book = await _bookService.GetBookAndReviewsByIdAsync(model.BookId);

            var vm = new ReviewListViewModel
            {
                BookId = book.Id,
                BookTitle = book.Title,
                Reviews = book.Reviews.Select(r => r.ToResource()).ToList(),
                NewReview = model.NewReview
            };

            return View("Reviews", vm.BookId);
        }

        return RedirectToAction("Reviews", new { id = result.BookId });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Vote(long reviewId, bool isUpvote)
    {
        var userId = User.GetUserId();
        var success = await _reviewService.VoteAsync(reviewId, userId, isUpvote);
        var review = await _reviewService.GetByIdAsync(reviewId);
        if (review is null)
        {
            ModelState.AddModelError(string.Empty, "Error voting review");
            return RedirectToAction("Index");
        }
        return RedirectToAction("Reviews", new { id = review.BookId });
    }

    public IActionResult Create()
    {
        return View(new BookFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var entity = Book.Create(model.Title, model.Author, model.PublishedYear, model.Genre);
        var book = await _bookService.AddAsync(entity);
        if (book.Id == 0)
        {
            ModelState.AddModelError("", "Failed to create book.");
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(long id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book == null) return NotFound();

        var model = new BookFormViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            PublishedYear = book.PublishedYear
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BookFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var book = await _bookService.GetByIdAsync(model.Id);
        if (book == null) return NotFound();
        book.Author = model.Author;
        book.Title = model.Title;
        book.PublishedYear = model.PublishedYear;
        book.Genre = model.Genre;

        var success = await _bookService.UpdateAsync(book);
        if (book.Id==0)
        {
            ModelState.AddModelError("", "Failed to update book.");
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book == null) return NotFound();
        var success = await _bookService.RemoveAsync(book);
        if (!success)
        {
            TempData["Error"] = "Failed to delete book.";
        }

        return RedirectToAction(nameof(Index));
    }
}