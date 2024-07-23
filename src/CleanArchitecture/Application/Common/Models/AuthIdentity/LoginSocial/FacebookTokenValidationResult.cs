using Newtonsoft.Json;

namespace CleanArchitecture.Application.Common.Models.AuthIdentity.LoginSocial;

public class FacebookTokenValidationResult
{
    [JsonProperty("data")]
    public FacebookTokenValidationData Data { get; set; }
}

public class FacebookTokenValidationData
{
    [JsonProperty("app_id")]
    public string AppId { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("application")]
    public string Application { get; set; }

    [JsonProperty("data_access_expires_at")]
    public int DataAccessExpiresAt { get; set; }

    [JsonProperty("expires_at")]
    public int ExpiresAt { get; set; }

    [JsonProperty("is_valid")]
    public bool IsValid { get; set; }

    [JsonProperty("scopes")]
    public List<string> Scopes { get; set; }

    [JsonProperty("user_id")]
    public string UserId { get; set; }
}

