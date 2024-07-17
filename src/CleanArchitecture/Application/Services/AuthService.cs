using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models.User;
using CleanArchitecture.Application.Common.Utilities;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Infrastructure.Interface;

namespace CleanArchitecture.Application.Services;
public class AuthService(IUnitOfWork unitOfWork,
                         IMapper mapper,
                         ITokenService tokenService,
                         ICurrentUser currentUser,
                         IUserRepository userRepository,
                         ICookieService cookieService
                         ) : IAuthService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ICookieService _cookieService = cookieService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<UserSignInResponse> SignIn(UserSignInRequest request)
    {
        var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.UserName == request.UserName)
            ?? throw UserException.BadRequestException(UserErrorMessage.UserNotExist);

        if (!StringHelper.Verify(request.Password, user.Password))
        {
            throw UserException.BadRequestException(UserErrorMessage.PasswordIncorrect);
        }

        var token = _tokenService.GenerateToken(user);
        _cookieService.Set(token);

        var response = _mapper.Map<UserSignInResponse>(user);
        response.Token = token;

        return response;
    }

    public async Task<UserSignUpResponse> SignUp(UserSignUpRequest request, CancellationToken token)
    {
        var isUserNameExist = await _unitOfWork.UserRepository.AnyAsync(x => x.UserName == request.UserName);
        if (isUserNameExist)
            throw UserException.UserAlreadyExistsException(request.UserName);

        var isEmailExist = await _unitOfWork.UserRepository.AnyAsync(x => x.UserName == request.Email);
        if (isEmailExist)
            throw UserException.UserAlreadyExistsException(request.Email);

        var user = _mapper.Map<User>(request);
        user.Password = user.Password.Hash();
        await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.UserRepository.AddAsync(user), token);

        var response = _mapper.Map<UserSignUpResponse>(user);

        return response;
    }

    public void Logout()
    {
        try
        {
            _ = _cookieService.Get();
            _cookieService.Delete();
        }
        catch { }
    }

    public async Task<UserProfileResponse> GetProfile()
    {
        var userId = _currentUser.GetCurrentUserId();
        var user = await _userRepository.GetByIdAsync(userId);

        var result = _mapper.Map<UserProfileResponse>(user);
        return result;
    }

    public async Task<string> RefreshToken()
    {
        var user = await _userRepository.GetByIdAsync(_currentUser.GetCurrentUserId());
        var accessToken = _tokenService.GenerateToken(user);
        _cookieService.Set(accessToken);

        return accessToken;
    }
}
