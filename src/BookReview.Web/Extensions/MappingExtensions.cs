using BookReview.ApplicationCore.Domain;
using BookReview.Web.Models.Resources;

namespace BookReview.Web.Extensions;

public static class MappingExtensions
{
    public static BookResource ToResource(this Book book)
    {
        return new BookResource
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            PublishedYear = book.PublishedYear,
            Genre = book.Genre,
            TotalReviews = book.Reviews.Count,
            AverageRating = book.Reviews.Count > 0 ? book.Reviews.Average(r => r.Rating) : 0,
            Reviews = book.Reviews.Select(r => r.ToResource()).ToList()
        };
    }

    public static ReviewResource ToResource(this Review review)
    {
        return new ReviewResource
        {
            Id = review.Id,
            Content = review.Content,
            Rating = review.Rating,
            DateCreated = review.DateCreated,
            BookId = review.BookId,
            BookTitle = review.Book?.Title,
            UserId = review.UserId,
            Username = review.User?.UserName,
            Upvotes = review.Votes.Count(v => v.IsUpvote),
            Downvotes = review.Votes.Count(v => !v.IsUpvote)
        };
    }

    public static ReviewVoteResource ToResource(this ReviewVote vote)
    {
        return new ReviewVoteResource
        {
            Id = vote.Id,
            ReviewId = vote.ReviewId,
            UserId = vote.UserId,
            Username = vote.User?.UserName,
            IsUpvote = vote.IsUpvote
        };
    }

    public static BookFilter ToDomain(this BookFilterResource vm)
    {
        int? skip = null;
        int? take = null;

        if (vm.Page > 0 && vm.PageSize > 0)
        {
            skip = (vm.Page - 1) * vm.PageSize;
            take = vm.PageSize;
        }

        return new BookFilter
        {
            TitleContains = vm.Title,
            AuthorContains = vm.Author,
            MinAverageRating = vm.MinRating,
            MaxAverageRating = vm.MaxRating,
            SortBy = vm.SortBy,
            Descending = vm.Descending,
            Skip = skip,
            Take = take
        };
    }
}
