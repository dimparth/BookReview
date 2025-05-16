namespace BookReview.ApplicationCore.Domain;

public sealed class Review
{
    public long Id { get; private set; }

    public string Content { get; private set; } = string.Empty;
    public int Rating { get; private set; }
    public DateTime DateCreated { get; private set; }

    public long BookId { get; private set; }
    public Book Book { get; private set; } = null!;

    public long UserId { get; private set; }
    public User User { get; private set; } = null!;

    private readonly List<ReviewVote> _votes = [];
    public IReadOnlyCollection<ReviewVote> Votes => _votes.AsReadOnly();

    private Review() { }

    public static Review Create(string content, int rating, long userId, int bookId)
    {
        if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Review content is required.");
        if (rating is < 1 or > 5) throw new ArgumentOutOfRangeException(nameof(rating));
        if (userId == 0) throw new ArgumentException("User is required.");

        return new Review
        {
            Content = content,
            Rating = rating,
            UserId = userId,
            BookId = bookId,
            DateCreated = DateTime.UtcNow
        };
    }

    public void AddVote(ReviewVote vote)
    {
        _votes.Add(vote);
    }
}
