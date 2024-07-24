// check third party login

// check send email
// upload image to cloudary
// check role and avatar

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Utilities;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using Payload = Google.Apis.Auth.GoogleJsonWebSignature.Payload;
using CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;

namespace CleanArchitecture.Application.Services;
public class AuthIdentityService(ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IFacebookAuthService facebookAuthService,
    IMapper mapper,
    ILogger<AuthIdentityService> logger,
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    IMailService emailSender,
    ICurrentUser currentUser) : IAuthIdentityService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IFacebookAuthService _facebookAuthService = facebookAuthService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<AuthIdentityService> _logger = logger;
    private readonly IMailService _emailSender = emailSender;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    //Refresh Token
    public async Task<TokenResult> RefreshTokenAsync(string token, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.Include(x => x.RefreshTokens).SingleOrDefaultAsync(
            x => x.Id == new Guid(_currentUser.GetCurrentStringUserId()),
            cancellationToken) ?? throw AuthIdentityException.ThrowAccountDoesNotExist();

        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (!refreshToken.IsActive)
        {
            throw AuthIdentityException.ThrowTokenNotActive();
        }

        // recall current token
        refreshToken.Revoked = DateTime.UtcNow;

        var res = await _tokenService.GenerateToken(user, cancellationToken);

        var result = new TokenResult
        {
            UserId = res.UserId,
            Expires = res.Expires,
            Token = res.Token,
        };
        return result;
    }

    public async Task<TokenResult> Authenticate(LoginRequest request, CancellationToken cancellationToken)
    {

        var user = await _userManager.FindByNameAsync(request.UserName)
            ?? throw AuthIdentityException.ThrowAccountDoesNotExist();

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);

        if (!result.Succeeded)
        {
            throw AuthIdentityException.ThrowLoginUnsuccessful(result.ToString());
        }

        if (user.AvatarId != null)
        {
            var avatar = _unitOfWork.MediaRepository.FirstOrDefaultAsync(m => m.MediaId == user.AvatarId).Result.PathMedia;
            user.Avatar.PathMedia = avatar ?? string.Empty;
        }

        var token = await _tokenService.GenerateToken(user, cancellationToken);

        return token;
    }

    public async Task Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user != null)
        {
            throw AuthIdentityException.ThrowUsernameAvailable();
        }

        user = await _userManager.FindByEmailAsync(request.Email);

        if (user != null)
        {
            throw AuthIdentityException.ThrowEmailAvailable();
        }

        user = new ApplicationUser()
        {
            Email = request.Email,
            UserName = request.UserName,
            Name = request.Name
        };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, Role.User.ToString());
        }
        else
        {
            List<IdentityError> errorList = result.Errors.ToList();
            var errors = string.Join(", ", errorList.Select(e => e.Description));
            throw AuthIdentityException.ThrowRegisterUnsuccessful(errors);
        }
    }

    public async Task<UserViewModel> Get(CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.Include(x => x.RefreshTokens).SingleOrDefaultAsync(
            x => x.Id == new Guid(_currentUser.GetCurrentStringUserId()),
            cancellationToken) ?? throw AuthIdentityException.ThrowAccountDoesNotExist();
        var roles = await _userManager.GetRolesAsync(user);

        var result = new UserViewModel
        {
            UserId = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            FullName = user.Name,
            Roles = roles,
            Avatar = user.Avatar.PathMedia
        };
        return result;
    }

    //Gán quyền người dùng
    public async Task RoleAssign(RoleAssignRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId)
        ?? throw AuthIdentityException.ThrowAccountDoesNotExist();

        var removedRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList();

        foreach (var roleName in removedRoles)
        {
            if (await _userManager.IsInRoleAsync(user, roleName) == true)
            {
                await _userManager.RemoveFromRoleAsync(user, roleName);
            }
        }

        await _userManager.RemoveFromRolesAsync(user, removedRoles);

        var addedRoles = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();

        foreach (var roleName in addedRoles)
        {
            if (await _userManager.IsInRoleAsync(user, roleName) == false)
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }
    }


    //Đăng nhập bằng Facebook
    public async Task<TokenResult> SignInFacebook(string accessToken, CancellationToken cancellationToken)
    {
        var validatedTokenResult = await _facebookAuthService.ValidationAcessTokenAsync(accessToken);

        if (!validatedTokenResult.Data.IsValid)
        {
            throw AuthIdentityException.ThrowInvalidFacebookToken();
        }

        //Lấy thông tin User Facebook từ AccessToken
        var userInfo = await _facebookAuthService.GetUsersInfoAsync(accessToken);

        //Cắt Email thành UsernName
        string userName = (userInfo.Email).Split('@')[0];

        //Tạo FullName
        string fullName = (userInfo.LastName + userInfo.FirstName).ToString();

        //Kiểm tra user đã liên kết với Facebook chưa
        var checkLinked = await _signInManager.ExternalLoginSignInAsync("Facebook", userInfo.Id, false);

        //Nếu đã có liên kết trước đó (Email tồn tại) thì tiến hành đăng nhập luôn
        if (checkLinked.Succeeded)
        {
            var exist_user = await _userManager.FindByEmailAsync(userInfo.Email);

            var avatar = _context.Media.Where(m => m.MediaId == exist_user.AvatarId).Select(m => m.PathMedia)
                .FirstOrDefault();

            var tokenResult = await _tokenService.GenerateToken(exist_user, cancellationToken);

            return tokenResult;
        }
        else //Chưa được liên kết với Facebook
        {
            var info = new ExternalLoginInfo(ClaimsPrincipal.Current, "Facebook", userInfo.Id, null);

            //Kiểm tra tồn tại Email
            var exist_user = await _userManager.FindByEmailAsync(userInfo.Email);

            if (exist_user != null)
            {
                //Gán tài khoản Facebook vào tài khoản đã có sẵn
                var result = await _userManager.AddLoginAsync(exist_user, info);
                if (result.Succeeded)
                {
                    var avatar = _context.Media.Where(m => m.MediaId == exist_user.AvatarId)
                        .Select(m => m.PathMedia).FirstOrDefault();

                    var tokenResult = await _tokenService.GenerateToken(exist_user, cancellationToken);

                    return tokenResult;
                }
                else
                {
                    throw AuthIdentityException.ThrowErrorLinkedFacebook();
                }
            }
            else //Tài khoản đăng nhập lần đầu
            {
                var indentityuser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    Email = userInfo.Email,
                    UserName = userName,
                    Name = fullName
                };

                var result = await _userManager.CreateAsync(indentityuser);

                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(indentityuser, info);

                    if (!result.Succeeded)
                    {
                        throw AuthIdentityException.ThrowErrorLinkedFacebook();
                    }

                    var roles = await _userManager.AddToRoleAsync(indentityuser, "Subscriber");

                    var tokenResult = await _tokenService.GenerateToken(exist_user, cancellationToken);
                    return tokenResult;
                }
                else
                {
                    List<IdentityError> errorList = result.Errors.ToList();
                    var errors = string.Join(", ", errorList.Select(e => e.Description));
                    throw AuthIdentityException.ThrowRegisterFacebookUnsuccessful(errors);
                }
            }
        }
    }

    //Đăng nhập bằng Google
    public async Task<TokenResult> SignInGoogle(string accessToken, CancellationToken cancellationToken)
    {

        Payload payload = await ValidateAsync(accessToken)
            ?? throw AuthIdentityException.ThrowErrorLinkedGoogle();


        //Kiểm tra user đã liên kết với Google chưa
        var checkLinked = await _signInManager.ExternalLoginSignInAsync("Google", payload.Subject, false);

        //Cắt Email thành UserName
        string userName = (payload.Email).Split('@')[0];

        //Tạo FullName
        string fullName = (payload.FamilyName + payload.GivenName).ToString();

        //Nếu đã có liên kết trước đó (Email) thì tiến hành đăng nhập luôn
        if (checkLinked.Succeeded)
        {
            var exist_user = await _userManager.FindByEmailAsync(payload.Email);

            var avatar = _context.Media.Where(m => m.MediaId == exist_user.AvatarId).Select(m => m.PathMedia)
                .FirstOrDefault();

            var tokenResult = await _tokenService.GenerateToken(exist_user, cancellationToken);

            return tokenResult;
        }
        else //Chưa được liên kết với Google
        {
            var info = new ExternalLoginInfo(ClaimsPrincipal.Current, "Google", payload.Subject, null);

            //Kiểm tra tồn tại Email và Username
            var exist_user = await _userManager.FindByEmailAsync(payload.Email);

            if (exist_user != null)
            {
                //Gán tài khoản Facebook vào tài khoản đã có sẵn
                var result = await _userManager.AddLoginAsync(exist_user, info);

                if (result.Succeeded)
                {
                    var avatar = _context.Media.Where(m => m.MediaId == exist_user.AvatarId)
                        .Select(m => m.PathMedia).FirstOrDefault();

                    var tokenResult = await _tokenService.GenerateToken(exist_user, cancellationToken);


                    return tokenResult;
                }
                else
                {
                    throw AuthIdentityException.ThrowErrorLinkedGoogle();
                }
            }
            else //Tài khoản đăng nhập lần đầu
            {
                var indentityuser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    Email = payload.Email,
                    UserName = userName,
                    Name = fullName
                };

                var result = await _userManager.CreateAsync(indentityuser);

                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(indentityuser, info);

                    if (!result.Succeeded)
                    {
                        throw AuthIdentityException.ThrowErrorLinkedGoogle();
                    }

                    var roles = await _userManager.AddToRoleAsync(indentityuser, "Subscriber");


                    var tokenResult = await _tokenService.GenerateToken(exist_user, cancellationToken);

                    return tokenResult;
                }
                else
                {
                    List<IdentityError> errorList = result.Errors.ToList();
                    var errors = string.Join(", ", errorList.Select(e => e.Description));
                    throw AuthIdentityException.ThrowRegisterGoogleUnsuccessful(errors);
                }
            }
        }
    }

    //Đăng nhập bằng Apple
    public async Task<TokenResult> SignInApple(string fullName, string identityToken, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var payload = tokenHandler.ReadJwtToken(identityToken) ?? throw AuthIdentityException.ThrowInvalidAppleToken();

        //Kiểm tra user đã liên kết với Google chưa
        var checkLinked = await _signInManager.ExternalLoginSignInAsync("Apple", payload.Subject, false);

        //Cắt Email thành UserName
        string email = payload.Payload.SingleOrDefault(x => x.Key == "email").Value.ToString()
            ?? throw AuthIdentityException.ThrowEmailRequired();

        string userName = email.Split('@')[0];

        //Nếu đã có liên kết trước đó (Email) thì tiến hành đăng nhập luôn
        if (checkLinked.Succeeded)
        {
            var exist_user = await _userManager.FindByEmailAsync(email);

            var avatar = _context.Media.Where(m => m.MediaId == exist_user.AvatarId).Select(m => m.PathMedia)
                .FirstOrDefault();

            var tokenResult = await _tokenService.GenerateToken(exist_user, cancellationToken);


            return tokenResult;
        }
        else //Chưa được liên kết với Apple
        {
            var info = new ExternalLoginInfo(ClaimsPrincipal.Current, "Apple", payload.Subject, null);

            //Kiểm tra tồn tại Email và Username
            var exist_user = await _userManager.FindByEmailAsync(email);

            if (exist_user != null)
            {
                //Gán tài khoản Apple vào tài khoản đã có sẵn
                var result = await _userManager.AddLoginAsync(exist_user, info);

                if (result.Succeeded)
                {
                    var avatar = _context.Media.Where(m => m.MediaId == exist_user.AvatarId)
                        .Select(m => m.PathMedia).FirstOrDefault();


                    var tokenResult = await _tokenService.GenerateToken(exist_user, cancellationToken);

                    return tokenResult;
                }
                else
                {
                    throw AuthIdentityException.ThrowErrorLinkedApple();
                }
            }
            else //Tài khoản đăng nhập lần đầu
            {
                var indentityuser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    UserName = userName,
                    Name = fullName
                };

                var result = await _userManager.CreateAsync(indentityuser);

                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(indentityuser, info);

                    if (!result.Succeeded)
                    {
                        throw AuthIdentityException.ThrowErrorLinkedApple();
                    }

                    var roles = await _userManager.AddToRoleAsync(indentityuser, "Subscriber");

                    var tokenResult = await _tokenService.GenerateToken(exist_user, cancellationToken);

                    return tokenResult;
                }
                else
                {
                    List<IdentityError> errorList = result.Errors.ToList();
                    var errors = string.Join(", ", errorList.Select(e => e.Description));
                    throw AuthIdentityException.ThrowRegisterAppleUnsuccessful(errors);
                }
            }
        }
    }

    public async Task<ForgotPassword> SendPasswordResetCode(SendPasswordResetCodeRequest request, CancellationToken cancellationToken)
    {
        //Get identity user details user manager
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw AuthIdentityException.ThrowUserNotFound();

        //Generate password reset token
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        //Generate OTP
        int otp = StringHelper.GenerateRandom(100000, 999999);

        var resetPassword = new ForgotPassword()
        {
            Email = request.Email,
            OTP = otp.ToString(),
            Token = token,
            UserId = user.Id,
            DateTime = DateTime.Now
        };

        //save data into db with OTP
        await _unitOfWork.ExecuteTransactionAsync(
            async () => await _unitOfWork.ForgotPasswordRepository.AddAsync(resetPassword), cancellationToken);

        //To do: Send token in email
        await _emailSender.SendEmailAsync(request.Email, "Reset Password OTP", "Hello "
            + request.Email +
            "<br><br>Please find the reset password token below<br><br><b>"
            + otp +
            "<b><br><br>Thanks<br>truongnhon.tk");

        return resetPassword;
    }

    public async Task<ForgotPassword> ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        //Get identity user details user manager
        var user = await _userManager.FindByEmailAsync(request.Email);

        //Getting token from otp
        var resetPasswordDetails = await _context.ForgotPassword
            .Where(x => x.OTP == request.OTP && x.UserId == user.Id)
            .OrderByDescending(x => x.DateTime).FirstOrDefaultAsync();

        //Verify if token is older than 3 minutes
        var expirationDateTime = resetPasswordDetails.DateTime.AddMinutes(3);

        if (expirationDateTime < DateTime.Now)
        {
            throw AuthIdentityException.ThrowGenerateTheNewOTP();
        }

        var res = await _userManager.ResetPasswordAsync(user, resetPasswordDetails.Token, request.NewPassword);

        return !res.Succeeded ? throw AuthIdentityException.ThrowOTPWrong() : new ForgotPassword();
    }
}
