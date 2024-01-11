using Clean.Architecture.Application.Common.Models.User;

namespace Clean.Architecture.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> Authenticate(LoginRequest request);
        Task<UserDTO> Register(RegisterRequest request, CancellationToken token);
    }
}
