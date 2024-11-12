using System.Security.Claims;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Utilities;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Shared.Models.AuthIdentity.UsersIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Services;
public class AuthIdentityService(ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    IMailService emailSender,
    ICurrentUser currentUser,
    AppSettings appSettings,
    ICookieService cookieService) : IAuthIdentityService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IMailService _emailSender = emailSender;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly AppSettings _appSettings = appSettings;
    private readonly ICookieService _cookieService = cookieService;

    public async Task LogOut()
    {
        _cookieService.Delete();
        await _signInManager.SignOutAsync();
    }

    public async Task<TokenResult> Authenticate(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .Include(u => u.Avatar)
            .FirstOrDefaultAsync(u => u.UserName == request.UserName, cancellationToken)
            ?? throw AuthIdentityException.ThrowAccountDoesNotExist();

        // Step 2: Check the password first to avoid unnecessary database queries if invalid.
        var passwordCheckResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (!passwordCheckResult.Succeeded)
        {
            throw AuthIdentityException.ThrowLoginUnsuccessful();
        }

        // Step 3: Retrieve user's claims in bulk to avoid multiple individual queries.
        var userClaims = await _userManager.GetClaimsAsync(user);
        var scopes = userClaims.FirstOrDefault(c => c.Type == "scope")?.Value.Split(' ') ?? Array.Empty<string>();

        // Step 4: Generate authentication token.
        var token = await _tokenService.GenerateToken(user, scopes, cancellationToken);
        _cookieService.Delete();
        _cookieService.Set(token.Token);

        return token;
    }

    public async Task Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByNameAsync(request.UserName) != null)
            throw AuthIdentityException.ThrowUsernameAvailable();

        if (await _userManager.FindByEmailAsync(request.Email) != null)
            throw AuthIdentityException.ThrowEmailAvailable();

        var user = new ApplicationUser()
        {
            Email = request.Email,
            UserName = request.UserName,
            Name = request.Name
        };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            List<IdentityError> errorList = result.Errors.ToList();
            var errors = string.Join(", ", errorList.Select(e => e.Description));
            throw AuthIdentityException.ThrowRegisterUnsuccessful(errors);
        }

        await _userManager.AddToRoleAsync(user, Role.User.ToString());
        // Add custom scope claim to the user
        string readScope = _appSettings.Jwt.ScopeBaseDomain + "/read";
        string writeScope = _appSettings.Jwt.ScopeBaseDomain + "/write";
        string[] scopes = [readScope, writeScope];

        // Add custom scope claim to the user
        var scopeClaim = new Claim("scope", string.Join(" ", scopes)); // Space-separated scopes

        await _userManager.AddClaimAsync(user, scopeClaim);
    }

    //Refresh Token
    public async Task<TokenResult> RefreshTokenAsync(string token, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.Include(x => x.RefreshTokens)
                                           .SingleOrDefaultAsync(x => x.Id == new Guid(_currentUser.GetCurrentStringUserId()), cancellationToken) ?? throw AuthIdentityException.ThrowAccountDoesNotExist();

        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (!refreshToken.IsActive)
            throw AuthIdentityException.ThrowTokenNotActive();

        // recall current token
        refreshToken.Revoked = DateTime.UtcNow;

        // Retrieve user's claims, including scope claim
        var userClaims = await _userManager.GetClaimsAsync(user);
        var scopeClaim = userClaims.FirstOrDefault(c => c.Type == "scope");

        // Extract scopes from the claim, if it exists
        var scopes = scopeClaim?.Value.Split(' ') ?? [];

        var result = await _tokenService.GenerateToken(user, scopes, cancellationToken);
        _cookieService.Delete();
        _cookieService.Set(result.Token);
        return result;
    }

    public async Task<UserViewModel> Get(CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .Include(u => u.Avatar).Select(x => new UserViewModel
                    {
                        Id = x.Id,
                        Email = x.Email,
                        UserName = x.UserName,
                        FullName = x.Name,
                        Roles = x.UserRoles.Select(ur => ur.Role.Name).ToList(),
                        Avatar = x.Avatar.PathMedia ?? " "
                    })
                    .SingleOrDefaultAsync(x => x.Id == new Guid(_currentUser.GetCurrentStringUserId()), cancellationToken)
                    ?? throw AuthIdentityException.ThrowAccountDoesNotExist();

        return users;
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
            "<b><br><br>Thanks<br>nhonvo.github.io");

        return resetPassword;
    }

    public async Task ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        //Get identity user details user manager
        var user = await _userManager.FindByEmailAsync(request.Email);

        //Getting token from otp
        var resetPasswordDetails = await _unitOfWork.ForgotPasswordRepository.FirstOrDefaultAsync(
            filter: x => x.OTP == request.OTP && x.UserId == user.Id, x => x.DateTime, false);

        //Verify if token is older than 3 minutes
        var expirationDateTime = resetPasswordDetails.DateTime.AddMinutes(3);

        if (expirationDateTime < DateTime.Now)
        {
            throw AuthIdentityException.ThrowGenerateTheNewOTP();
        }

        var res = await _userManager.ResetPasswordAsync(user, resetPasswordDetails.Token, request.NewPassword);

        if (!res.Succeeded)
            throw AuthIdentityException.ThrowOTPWrong();
    }
}
