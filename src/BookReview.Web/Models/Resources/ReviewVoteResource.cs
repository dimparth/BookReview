namespace BookReview.Web.Models.Resources;

public sealed class ReviewVoteResource
{
    public long Id { get; set; }
    public long ReviewId { get; set; }
    public long UserId { get; set; }
    public string? Username { get; set; }
    public bool IsUpvote { get; set; }
}
