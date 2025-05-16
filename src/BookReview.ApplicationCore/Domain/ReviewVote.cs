namespace BookReview.ApplicationCore.Domain;

public sealed class ReviewVote
{
    public long Id { get; private set; }

    public long ReviewId { get; private set; }
    public Review Review { get; private set; } = null!;

    public long UserId { get; private set; }
    public User User { get; private set; } = null!;

    public bool IsUpvote { get; private set; }

    private ReviewVote() { }

    public static ReviewVote Create(long userId, long reviewId, bool isUpvote)
    {
        if (userId == 0) throw new ArgumentException("User ID is required.");

        return new ReviewVote
        {
            UserId = userId,
            ReviewId = reviewId,
            IsUpvote = isUpvote
        };
    }

    public void Toggle()
    {
        IsUpvote = !IsUpvote;
    }
}
