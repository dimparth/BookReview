namespace BookReview.ApplicationCore.Domain;

public sealed class Book
{
    public long Id { get; private set; }

    public string Title { get; private set; }
    public string Author { get; private set; }
    public int PublishedYear { get; private set; }
    public string Genre { get; private set; }

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

    public void AddReview(Review review)
    {
        _reviews.Add(review);
    }
}
