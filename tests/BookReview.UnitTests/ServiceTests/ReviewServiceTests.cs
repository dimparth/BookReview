using BookReview.ApplicationCore.Domain;
using BookReview.ApplicationCore.Interfaces;
using BookReview.UnitTests.Setup;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.UnitTests.ServiceTests;

public sealed class ReviewServiceTests : IClassFixture<ContainerFixture>
{
    private readonly IReviewService _service;
    private readonly IReviewRepository _reviewRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReviewServiceTests(ContainerFixture fixture)
    {
        _service = fixture.ServiceProvider.GetRequiredService<IReviewService>();
        _reviewRepository = fixture.ServiceProvider.GetRequiredService<IReviewRepository>();
        _bookRepository = fixture.ServiceProvider.GetRequiredService<IBookRepository>();
        _unitOfWork = fixture.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }

    [Fact]
    public async Task AddAsync_ShouldAddReview()
    {
        // Arrange
        var originalBook = Book.Create("Test Book", "Author", 2024, "ISBN");
        var book = Book.Create("Test Book", "Author", 2024, "ISBN");
        var user = User.Create("sample", "sample@sample.com");
        user.Id = 1;
        var review = Review.Create("Good", 4, user, book);
        _bookRepository.GetBookAndReviewsByIdAsync(Arg.Any<long>()).Returns(originalBook);
        var reviewReturn = book.AddReview(review.Content, review.Rating, user);
        _reviewRepository.AddAsync(Arg.Any<Review>()).Returns(reviewReturn);

        // Act
        var result = await _service.AddAsync(review);

        // Assert
        Assert.Equal(review.Content, result.Content);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAll()
    {
        var list = new List<Review> { Review.Create("Nice", 5, 1, 1) };
        _reviewRepository.GetAllAsync().Returns(list);

        var result = await _service.GetAllAsync();

        Assert.Equal(list, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectReview()
    {
        var review = Review.Create("Nice", 5, 1, 1);
        _reviewRepository.GetReviewWithVotesByIdAsync(1).Returns(review);

        var result = await _service.GetByIdAsync(1);

        Assert.Equal(review, result);
    }

    [Fact]
    public async Task RemoveAsync_ShouldDeleteReview()
    {
        var review = Review.Create("Nice", 5, 1, 1);
        _reviewRepository.DeleteAsync(review).Returns(true);

        var result = await _service.RemoveAsync(review);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateReview()
    {
        var review = Review.Create("Nice", 5, 1, 1);
        _reviewRepository.UpdateAsync(review).Returns(review);

        var result = await _service.UpdateAsync(review);

        Assert.Equal(review, result);
    }

    [Fact]
    public async Task VoteAsync_ShouldToggleVoteIfDifferent()
    {
        var review = Review.Create("Nice", 5, 1, 1);
        var vote = ReviewVote.Create(1, review.Id, true);
        review.AddVote(vote);

        _reviewRepository.GetReviewWithVotesByIdAsync(review.Id).Returns(review);

        var result = await _service.VoteAsync(review.Id, 1, false);

        Assert.True(result);
        Assert.False(vote.IsUpvote);
    }

    [Fact]
    public async Task VoteAsync_ShouldAddVoteIfNotExists()
    {
        var review = Review.Create("Nice", 5, 1, 1);
        _reviewRepository.GetReviewWithVotesByIdAsync(review.Id).Returns(review);

        var result = await _service.VoteAsync(review.Id, 1, true);

        Assert.True(result);
        Assert.Single(review.Votes);
        Assert.Equal(1, review.Votes.First().UserId);
        Assert.True(review.Votes.First().IsUpvote);
    }
}
