using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models.AuthIdentity.LoginSocial;
using Newtonsoft.Json;

namespace CleanArchitecture.Application.Services;

public class FacebookAuthService(IHttpClientFactory httpClient) : IFacebookAuthService
{

    private const string UserInfoUrl = "https://graph.facebook.com/me?fields=first_name,last_name,picture,email&access_token={0}";

    private readonly IHttpClientFactory _httpClientFactory = httpClient;

    public async Task<FacebookUserInfoResult> GetUsersInfoAsync(string accessToken)
    {
        var formattedUrl = string.Format(UserInfoUrl, accessToken);

        var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);

        result.EnsureSuccessStatusCode();

        var responseAsString = await result.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<FacebookUserInfoResult>(responseAsString);
    }


    public async Task<FacebookTokenValidationResult> ValidationAcessTokenAsync(string accessToken)
    {
        string AppId = "1001624143838277";
        string AppSecret = "a78ba77845f78f80b32337227240def5";

        string formattedUrl = $"https://graph.facebook.com/debug_token?input_token={accessToken}&access_token={AppId}|{AppSecret}";

        var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);

        result.EnsureSuccessStatusCode();

        var responseAsString = await result.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<FacebookTokenValidationResult>(responseAsString);
    }
}
