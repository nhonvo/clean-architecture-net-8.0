using CleanArchitecture.Shared.Models;

namespace CleanArchitecture.Domain.Entities;

public class Book : BaseModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double? Price { get; set; }
}
