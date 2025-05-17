namespace BookReview.Web.Models.Resources;

public sealed class AddVoteResource
{
    public long UserId { get; set; }
    public bool IsUpvote { get; set; }
}
