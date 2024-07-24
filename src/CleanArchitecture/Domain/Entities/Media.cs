namespace CleanArchitecture.Domain.Entities;

public class Media
{
    public int MediaId { get; set; }

    public MediaType Type { get; set; }

    public string PathMedia { get; set; } = "default.png";

    public string Caption { get; set; }

    public int SortOrder { get; set; }

    public int Duration { get; set; }

    public long FileSize { get; set; }

    public DateTime DateCreated { get; set; }
    public ApplicationUser User { get; set; }
}
