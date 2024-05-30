using System.Security.Claims;
using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models.User;
using CleanArchitecture.Application.Common.Utilities;
using CleanArchitecture.Infrastructure.Interface;

namespace CleanArchitecture.Application.Services
{
    public class UserService(IUnitOfWork unitOfWork,
                             IMapper mapper,
                             ITokenService tokenService,
                             ICurrentUser currentUser,
                             IUserRepository userRepository,
                             IHttpContextAccessor httpContextAccessor
                             ) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ITokenService _tokenService = tokenService;
        private readonly ICurrentUser _currentUser = currentUser;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<UserSignInResponse> SignIn(UserSignInRequest request)
        {
            var isUserExist = await _unitOfWork.UserRepository.AnyAsync(x => x.UserName == request.UserName);
            if (!isUserExist)
            {
                throw new UserFriendlyException(ErrorCode.BadRequest, "User does not exist!");
            }

            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.UserName == request.UserName);

            if (!StringHelper.Verify(request.Password, user.Password))
            {
                throw new UserFriendlyException(ErrorCode.BadRequest, "Password Incorrect!");
            }

            var token = _tokenService.GenerateToken(user);
            // Set cookies
            _httpContextAccessor.HttpContext.Response.Cookies.Append("acc", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None, // Or SameSiteMode.Lax if not using HTTPS
                Secure = true, // Set to true if using HTTPS
                MaxAge = TimeSpan.FromMinutes(30)
            });

            var response = _mapper.Map<UserSignInResponse>(user);
            response.Token = token;

            return response;
        }

        public async Task<UserSignUpResponse> SignUp(UserSignUpRequest request, CancellationToken token)
        {
            var isUserExist = await _unitOfWork.UserRepository.AnyAsync(x => x.UserName == request.UserName);
            if (isUserExist)
                throw new UserFriendlyException(ErrorCode.BadRequest, "This UserName Already Used");

            var isEmailExist = await _unitOfWork.UserRepository.AnyAsync(x => x.UserName == request.Email);
            if (isEmailExist)
                throw new UserFriendlyException(ErrorCode.BadRequest, "This Email Already Used!");

            var user = _mapper.Map<User>(request);
            user.Password = user.Password.Hash();
            await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.UserRepository.AddAsync(user), token);

            var response = _mapper.Map<UserSignUpResponse>(user);

            return response;
        }

        public void Logout()
        {
            var cookies = _httpContextAccessor.HttpContext.Request.Cookies;

            if (cookies.ContainsKey("acc"))
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("acc");
            }

            if (cookies.ContainsKey("ref"))
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("ref");
            }
        }

        public async Task<UserProfileResponse> GetProfile()
        {
            try
            {
                var jwtCookie = _httpContextAccessor.HttpContext.Request.Cookies["acc"];
                if (string.IsNullOrEmpty(jwtCookie))
                {
                    throw new UserFriendlyException(ErrorCode.Unauthorized, "user not logged in");
                }
                var token = _tokenService.ValidateToken(jwtCookie);
                var userId = token.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var user = await _userRepository.GetByIdAsync(int.Parse(userId));

                var result = _mapper.Map<UserProfileResponse>(user);
                return result;
            }
            catch (Exception exception)
            {
                throw new UserFriendlyException(ErrorCode.Internal, "something went wrong", exception);
            }
        }

        public async Task<string> RefreshToken()
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(_currentUser.GetCurrentUserId());

                var accessToken = _tokenService.GenerateToken(user);

                _httpContextAccessor.HttpContext.Response.Cookies.Append("ref", accessToken, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    MaxAge = TimeSpan.FromMinutes(30)
                });
                return accessToken;
            }
            catch (Exception exception)
            {
                throw new UserFriendlyException(ErrorCode.Internal, "something went wrong", exception);
            }
        }
    }
}
