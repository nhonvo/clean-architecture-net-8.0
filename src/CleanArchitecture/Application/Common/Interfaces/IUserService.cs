using CleanArchitecture.Application.Common.Models.User;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> Authenticate(LoginRequest request);
        Task<UserDTO> Register(RegisterRequest request, CancellationToken token);
    }
}
