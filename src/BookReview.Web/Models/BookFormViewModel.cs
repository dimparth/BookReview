using BookReview.ApplicationCore.Domain;
using System.ComponentModel.DataAnnotations;

namespace BookReview.Web.Models;

public class BookFormViewModel
{
    public long Id { get; set; }
    [Required] public string Title { get; set; } = string.Empty;
    [Required] public string Author { get; set; } = string.Empty;
    [Required][Range(0, 3000)] public int PublishedYear { get; set; }
    [Required] public string Genre { get; set; } = string.Empty;

    public static BookFormViewModel FromEntity(Book book) => new()
    {
        Title = book.Title,
        Author = book.Author,
        PublishedYear = book.PublishedYear,
        Genre = book.Genre
    };
}
