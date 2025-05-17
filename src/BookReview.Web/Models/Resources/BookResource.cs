namespace BookReview.Web.Models.Resources;

public sealed class BookResource
{
    public long Id { get; set; }
    public string Title { get; set; } = default!;
    public string Author { get; set; } = default!;
    public int PublishedYear { get; set; }
    public string Genre { get; set; } = default!;

    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }

    public List<ReviewResource> Reviews { get; set; } = [];
}
