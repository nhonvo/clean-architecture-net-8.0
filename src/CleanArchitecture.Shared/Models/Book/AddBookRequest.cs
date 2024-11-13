namespace CleanArchitecture.Shared.Models.Book;

public class BookDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double? Price { get; set; }
}

public class AddBookRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double? Price { get; set; }
}
public class UpdateBookRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double? Price { get; set; }
}
