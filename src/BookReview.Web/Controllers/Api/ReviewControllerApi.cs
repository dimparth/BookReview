using BookReview.ApplicationCore.Domain;
using BookReview.ApplicationCore.Interfaces;
using BookReview.Web.Extensions;
using BookReview.Web.Models.Resources;
using Microsoft.AspNetCore.Mvc;

namespace BookReview.Web.Controllers.Api;

[Route("api/reviews")]
[ApiController]
public class ReviewControllerApi : ControllerBase
{
    private readonly IReviewService _reviewService;
    public ReviewControllerApi(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpPost]
    public async Task<ActionResult<ReviewResource>> CreateReviewAsync([FromBody] CreateReviewResource review)
    {
        if (review is null)
        {
            return BadRequest("Review cannot be null");
        }
        var domain = Review.Create(
            review.Content ?? "",
            review.Rating,
            review.UserId,
            review.BookId
        );
        var createdReview = await _reviewService.AddAsync(domain);
        return CreatedAtAction(nameof(CreateReviewAsync), new { id = createdReview.Id }, createdReview.ToResource());
    }

    [HttpPost("api/reviews/{id:long}/vote")]
    public async Task<IActionResult> Vote(long id, [FromBody] AddVoteResource request)
    {
        var result = await _reviewService.VoteAsync(id, request.UserId, request.IsUpvote);

        return result ? Ok() : BadRequest();
    }
}
