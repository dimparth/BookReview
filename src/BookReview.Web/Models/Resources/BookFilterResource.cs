namespace BookReview.Web.Models.Resources;

public class BookFilterResource
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public double? MinRating { get; set; }
    public double? MaxRating { get; set; }

    public string? SortBy { get; set; } 
    public bool Descending { get; set; } = false;

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public bool IsEmpty()
    {
        return string.IsNullOrWhiteSpace(Title)
            && string.IsNullOrWhiteSpace(Author)
            && MinRating == null
            && MaxRating == null
            && string.IsNullOrWhiteSpace(SortBy)
            && Page == 1
            && PageSize == 10
            && Descending == false;
    }
}
