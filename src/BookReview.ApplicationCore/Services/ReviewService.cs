using BookReview.ApplicationCore.Domain;
using BookReview.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Cryptography;

namespace BookReview.ApplicationCore.Services;

public sealed class ReviewService : BaseService<Review, long>, IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IBookService _bookService;
    private readonly IUserService _userService;
    public ReviewService(ILogger<ReviewService> logger,
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork,
        IBookService bookService,
        IUserService userService) : base(logger, unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _bookService = bookService;
        _userService = userService;
    }
    protected override IRepository<Review, long> GetRepository()
    {
        return _reviewRepository;
    }

    public override async Task<Review?> GetByIdAsync(long id)
    {
        return await _reviewRepository.GetReviewWithVotesByIdAsync(id);
    }

    public override async Task<Review> AddAsync(Review entity)
    {
        var user = await _userService.GetByIdAsync(entity.UserId) ?? throw new Exception("User not found");
        var book = await _bookService.GetBookAndReviewsByIdAsync(entity.BookId) ?? throw new Exception("Book not found");

        if (book.Reviews.Any(r => r.UserId == entity.UserId))
            throw new Exception("User has already reviewed this book");

        var review = book.AddReview(entity.Content, entity.Rating, user);
        var added = await _reviewRepository.AddAsync(review);

        await _unitOfWork.SaveChangesAsync();
        return added;
    }

    public async Task<IEnumerable<Review>> GetByBookIdAsync(long bookId)
    {
        return await _reviewRepository.GetReviewsForBookAsync(bookId);
    }

    public async Task<bool> VoteAsync(long reviewId, long userId, bool isUpvote)
    {
        var review = await GetByIdAsync(reviewId);
        if (review is null)
            throw new Exception("Review not found.");

        var existingVote = review.Votes.FirstOrDefault(v => v.UserId == userId);

        if (existingVote is not null)
        {
            if (existingVote.IsUpvote != isUpvote)
                existingVote.Toggle();
        }
        else
        {
            var vote = ReviewVote.Create(userId, reviewId, isUpvote);
            review.AddVote(vote);
        }

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}

