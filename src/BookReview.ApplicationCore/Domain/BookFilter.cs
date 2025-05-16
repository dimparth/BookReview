namespace BookReview.ApplicationCore.Domain;

public sealed class BookFilter
{
    public string? TitleContains { get; set; }
    public string? AuthorContains { get; set; }
    public double? MinAverageRating { get; set; }
    public double? MaxAverageRating { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}
