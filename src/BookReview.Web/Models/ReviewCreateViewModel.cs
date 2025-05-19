using System.ComponentModel.DataAnnotations;

namespace BookReview.Web.Models;

public class ReviewCreateViewModel
{
    [Required]
    [StringLength(1000, MinimumLength = 2)]
    public string Content { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; }

    public long BookId { get; set; }
}
