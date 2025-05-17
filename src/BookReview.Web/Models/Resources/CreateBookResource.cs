namespace BookReview.Web.Models.Resources;

public sealed class CreateBookResource
{
    public string Title { get; set; } = default!;
    public string Author { get; set; } = default!;
    public int PublishedYear { get; set; }
    public string Genre { get; set; } = default!;
}
