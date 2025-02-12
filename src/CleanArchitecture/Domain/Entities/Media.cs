using CleanArchitecture.Shared.Models;

namespace CleanArchitecture.Domain.Entities;

public class Media : BaseModel
{
    public int MediaId { get; set; }
    public MediaType Type { get; set; }
    public string PathMedia { get; set; }
    public string Caption { get; set; }
    public long FileSize { get; set; }
    public DateTime DateCreated { get; set; }
    public ApplicationUser User { get; set; }
}
