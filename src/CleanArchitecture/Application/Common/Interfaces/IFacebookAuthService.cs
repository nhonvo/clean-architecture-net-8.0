using CleanArchitecture.Application.Common.Models.AuthIdentity.LoginSocial;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IFacebookAuthService
{
    Task<FacebookTokenValidationResult> ValidationAccessTokenAsync(string accessToken);
    Task<FacebookUserInfoResult> GetUsersInfoAsync(string accessToken);
}
