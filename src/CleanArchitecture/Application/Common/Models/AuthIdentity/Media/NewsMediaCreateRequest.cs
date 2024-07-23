namespace CleanArchitecture.Application.Common.Models.AuthIdentity.Media;

public class NewsMediaCreateRequest
{
    public MediaType Type { get; set; }

    public string MediaLink { get; set; }

    public string Caption { get; set; }

    public int Duration { get; set; }

    public DateTime DateCreated { get; set; }

    public IFormFile MediaFile { get; set; }

}
