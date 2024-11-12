using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Shared.Models.AuthIdentity.Media;

public class NewsMediaUpdateRequest
{
    public int MediaId { get; set; }

    public MediaType Type { get; set; }

    public string PathMedia { get; set; }

    public string Caption { get; set; }

    public int Duration { get; set; }

    public long FileSize { get; set; }

    public IFormFile MediaFile { get; set; }
}
