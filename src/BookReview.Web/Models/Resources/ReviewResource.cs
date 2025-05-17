namespace BookReview.Web.Models.Resources;

public sealed class ReviewResource
{
    public long Id { get; set; }
    public string Content { get; set; } = default!;
    public int Rating { get; set; }
    public DateTime DateCreated { get; set; }

    public long BookId { get; set; }
    public string? BookTitle { get; set; }

    public long UserId { get; set; }
    public string? Username { get; set; }

    public int Upvotes { get; set; }
    public int Downvotes { get; set; }
}
