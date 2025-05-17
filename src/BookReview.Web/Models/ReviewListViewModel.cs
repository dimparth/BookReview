using BookReview.Web.Models.Resources;

namespace BookReview.Web.Models;

public class ReviewListViewModel
{
    public long BookId { get; set; }

    public string BookTitle { get; set; } = string.Empty;

    public List<ReviewResource> Reviews { get; set; } = [];

    public ReviewCreateViewModel? NewReview { get; set; }
}
