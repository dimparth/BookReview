namespace BookReview.ApplicationCore.Domain;

public sealed class Book
{
    public long Id { get; private set; }

    public string Title { get; set; }
    public string Author { get; set; }
    public int PublishedYear { get; set; }
    public string Genre { get; set; }

    private readonly List<Review> _reviews = [];
    public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();

    private Book() { }

    public static Book Create(string title, string author, int publishedYear, string genre)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.");
        if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("Author is required.");
        if (string.IsNullOrWhiteSpace(genre)) throw new ArgumentException("Genre is required.");
        if (publishedYear < 0) throw new ArgumentException("Year must be valid.");

        return new Book
        {
            Title = title,
            Author = author,
            PublishedYear = publishedYear,
            Genre = genre
        };
    }

    public Review AddReview(string content, int rating, User user)
    {
        var review = Review.Create(content, rating, user, this);
        _reviews.Add(review);
        return review;
    }
}
