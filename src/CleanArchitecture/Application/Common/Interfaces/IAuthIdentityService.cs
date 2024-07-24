using CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IUserService
{
    Task<List<UserViewModel>> Get(CancellationToken cancellationToken);
    Task Update(UserUpdateRequest request, CancellationToken cancellationToken);
    Task Delete(string userId, CancellationToken cancellationToken);
}
public interface IAuthIdentityService
{
    Task<TokenResult> RefreshTokenAsync(string token, CancellationToken cancellationToken);
    Task<TokenResult> Authenticate(LoginRequest request, CancellationToken cancellationToken);
    Task Register(RegisterRequest request, CancellationToken cancellationToken);
    Task<UserViewModel> Get(CancellationToken cancellationToken);
    Task RoleAssign(RoleAssignRequest request, CancellationToken cancellationToken);
    Task<TokenResult> SignInFacebook(string accessToken, CancellationToken cancellationToken);
    Task<TokenResult> SignInGoogle(string accessToken, CancellationToken cancellationToken);
    Task<TokenResult> SignInApple(string fullName, string identityToken, CancellationToken cancellationToken);
    Task<ForgotPassword> SendPasswordResetCode(SendPasswordResetCodeRequest request, CancellationToken cancellationToken);
    Task<ForgotPassword> ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken);
}
