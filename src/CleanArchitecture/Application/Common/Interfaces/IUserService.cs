using CleanArchitecture.Application.Common.Models.User;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IUserService
{
    Task<UserSignInResponse> SignIn(UserSignInRequest request);
    Task<UserSignUpResponse> SignUp(UserSignUpRequest request, CancellationToken token);
    void Logout();
    Task<string> RefreshToken();
    Task<UserProfileResponse> GetProfile();
}
