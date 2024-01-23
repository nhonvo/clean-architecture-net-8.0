namespace CleanArchitecture.Application.Common.Models.Book;

public class BookDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double? Price { get; set; }
}
