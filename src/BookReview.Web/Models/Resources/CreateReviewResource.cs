namespace BookReview.Web.Models.Resources;

public sealed class CreateReviewResource
{
    public string? Content { get; set; } = default!;
    public int Rating { get; set; }
    public long BookId { get; set; }
    public long UserId { get; set; }
}
