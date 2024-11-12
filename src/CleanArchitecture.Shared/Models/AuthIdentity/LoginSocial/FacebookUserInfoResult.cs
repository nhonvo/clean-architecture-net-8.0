using Newtonsoft.Json;

namespace CleanArchitecture.Shared.Models.AuthIdentity.LoginSocial;

public class FacebookUserInfoResult
{
    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("last_name")]
    public string LastName { get; set; }

    [JsonProperty("picture")]
    public FacebookPictureData Picture { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }
}

public class FacebookPictureData
{
    [JsonProperty("height")]
    public int Height { get; set; }

    [JsonProperty("is_silhouette")]
    public bool IsSilhouette { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("width")]
    public int Width { get; set; }
}

public class FacebookPicture
{
    [JsonProperty("data")]
    public FacebookPictureData Data { get; set; }
}

