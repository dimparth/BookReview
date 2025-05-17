using BookReview.ApplicationCore.Domain;

namespace BookReview.Web.Models;

public class BookFilterViewModel
{
    public string? Genre { get; set; }
    public int? Year { get; set; }
    public double? MinRating { get; set; }
    public double? MaxRating { get; set; }

    public BookFilter ToFilter() => new()
    {
        Genre = Genre,
        PublishedYear = Year,
        MinAverageRating = MinRating,
        MaxAverageRating = MaxRating
    };
}
