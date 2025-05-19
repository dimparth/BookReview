using BookReview.ApplicationCore.Domain;
using BookReview.ApplicationCore.Interfaces;
using BookReview.ApplicationCore.Services;
using BookReview.UnitTests.Setup;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace BookReview.UnitTests.ServiceTests;

public sealed class BookServiceTests : IClassFixture<ContainerFixture>
{
    private readonly IBookService _bookService;
    private readonly IBookRepository _repo;
    private readonly IUnitOfWork _uow;

    public BookServiceTests(ContainerFixture fixture)
    {
        _bookService = fixture.ServiceProvider.GetRequiredService<IBookService>();
        _repo = fixture.ServiceProvider.GetRequiredService<IBookRepository>();
        _uow = fixture.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }

    [Fact]
    public async Task AddAsync_ShouldAddBookAndSave()
    {
        // Arrange
        var book = Book.Create("Test Title", "Test Author", 2024, "ISBN");
        _repo.AddAsync(book).Returns(book);

        // Act
        var result = await _bookService.AddAsync(book);

        // Assert
        Assert.Equal(book, result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBooks()
    {
        // Arrange
        var books = new List<Book> { Book.Create("Test", "Auth", 2024, "ISBN") };
        _repo.GetAllAsync().Returns(books);

        // Act
        var result = await _bookService.GetAllAsync();

        // Assert
        Assert.Equal(books, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectBook()
    {
        // Arrange
        var book = Book.Create("Test Title", "Test Author", 2024, "ISBN");
        _repo.GetByIdAsync(1).Returns(book);

        // Act
        var result = await _bookService.GetByIdAsync(1);

        // Assert
        Assert.Equal(book, result);
    }

    [Fact]
    public async Task RemoveAsync_ShouldDeleteBookAndSave()
    {
        // Arrange
        var book = Book.Create("Test Title", "Test Author", 2024, "ISBN");
        _repo.DeleteAsync(book).Returns(true);

        // Act
        var result = await _bookService.RemoveAsync(book);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateBookAndSave()
    {
        // Arrange
        var book = Book.Create("Test Title", "Test Author", 2024, "ISBN");
        _repo.UpdateAsync(book).Returns(book);

        // Act
        var result = await _bookService.UpdateAsync(book);

        // Assert
        Assert.Equal(book, result);   
    }

    [Fact]
    public async Task GetBookAndReviewsByIdAsync_ShouldCallRepository()
    {
        // Arrange
        var book = Book.Create("Test Title", "Test Author", 2024, "ISBN");
        _repo.GetBookAndReviewsByIdAsync(1).Returns(book);

        // Act
        var result = await _bookService.GetBookAndReviewsByIdAsync(1);

        // Assert
        Assert.Equal(book, result);
        await _repo.Received(1).GetBookAndReviewsByIdAsync(1);
    }

    [Fact]
    public async Task GetBooksByFiltersAsync_ShouldReturnFilteredBooks()
    {
        // Arrange
        var filter = new BookFilter { AuthorContains = "Test" };
        var book = Book.Create("Test Title", "Test Author", 2024, "ISBN");
        _repo.GetBooksByFilterAsync(filter).Returns([book]);

        // Act
        var result = await _bookService.GetBooksByFiltersAsync(filter);

        // Assert
        Assert.Single(result);
    }
}