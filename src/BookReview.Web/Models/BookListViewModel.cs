using BookReview.Web.Models.Resources;

namespace BookReview.Web.Models;

public class BookListViewModel
{
    public List<BookResource> Books { get; set; } = [];
    public BookFilterViewModel Filter { get; set; } = new();
}
