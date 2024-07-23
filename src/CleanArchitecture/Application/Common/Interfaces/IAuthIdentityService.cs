using CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IAuthIdentityService
{

    Task<TokenResult> Authenticate(LoginRequest request);
    Task Register(RegisterRequest request);
    Task<List<UserViewModel>> Get();
    Task<bool> Update(UserUpdateRequest request);
    Task<UserViewModel> GetById(Guid? id);
    Task<bool> Delete(string userId);
    Task<bool> RoleAssign(Guid id, RoleAssignRequest request);
    Task<TokenResult> SignInFacebook(string accessToken);
    Task<TokenResult> SignInGoogle(string accessToken);
    Task<TokenResult> SignInApple(string fullName, string identityToken);
    Task<ForgotPassword> SendPasswordResetCode(string Email);
    Task<ForgotPassword> ResetPassword(string email, string opt, string newPassword);
    Task<TokenResult> RefreshTokenAsync(string token);
    Task<UserViewModel> Profile();
}
