using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Utilities;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using Payload = Google.Apis.Auth.GoogleJsonWebSignature.Payload;
using CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;

namespace CleanArchitecture.Application.Services;


public class AuthIdentityService : IAuthIdentityService
{
    private readonly ApplicationDbContext _context;

    private readonly UserManager<ApplicationUser> _userManager;

    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly IFacebookAuthService _facebookAuthService;

    private readonly IMapper _mapper;

    private readonly RoleManager<RoleIdentity> _roleManager;

    private readonly IFileStorageService _storageService;

    private readonly Jwt _appSettings;
    private readonly ICurrentUser _user;
    private readonly ILogger<AuthIdentityService> _logger;
    private readonly IMailService _emailSender;

    public AuthIdentityService(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IFacebookAuthService facebookAuthService,
        IMapper mapper, RoleManager<RoleIdentity> roleManager, IFileStorageService storageService, AppSettings appSettings, ICurrentUser user, ILogger<AuthIdentityService> logger, IMailService emailSender)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _facebookAuthService = facebookAuthService;
        _mapper = mapper;
        _roleManager = roleManager;
        _storageService = storageService;
        _appSettings = appSettings.Jwt;
        _user = user;
        _logger = logger;
        _emailSender = emailSender;
        FileStorageService.USER_CONTENT_FOLDER_NAME = "images/avatars";
    }


    //Tạo Token
    private async Task<TokenResult> GenerateUserTokenAsync(ApplicationUser user, string avatar)
    {
        var token_result = new TokenResult();

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Uri, avatar ?? "default.png"),
            new Claim(ClaimTypes.GivenName, user.Name),
            new Claim(ClaimTypes.Role, roles == null ? "Subscriber" : string.Join(";", roles)),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddDays(10);

        var token = new JwtSecurityToken(
            _appSettings.Issuer,
            _appSettings.Audience,
            claims,
            expires: expires,
            signingCredentials: creds);

        var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);

        token_result.UserId = user.Id;

        token_result.Expires = expires;

        token_result.Token = tokenResult;

        var checkToken = _context.RefreshTokens.FirstOrDefault(f => f.UserId == user.Id);

        var refreshToken = new RefreshToken
        {
            Token = tokenResult,
            UserId = user.Id,
            Expires = expires,
            Created = DateTime.UtcNow
        };

        //check if refresh token is not exist, then add new one
        if (checkToken == null)
        {
            _context.RefreshTokens.Add(refreshToken);
        }
        //if refresh token is exist and valid, then update it
        else if (checkToken.Expires > DateTime.UtcNow)
        {
            checkToken.Token = tokenResult;
            checkToken.Expires = expires;
            checkToken.Created = DateTime.UtcNow;
        }
        //if refresh token is exist and expired, then delete it and add new one
        else
        {
            _context.RefreshTokens.Remove(checkToken);
            _context.RefreshTokens.Add(refreshToken);
        }
        await _context.SaveChangesAsync();
        return token_result;
    }

    //Refresh Token
    public async Task<TokenResult> RefreshTokenAsync(string token)
    {
        var user = _context.ApplicationUsers.Include(i => i.RefreshTokens)
                                 .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token))
                                 ?? throw new UserFriendlyException(ErrorCode.NotFound, "Token not exist");
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (!refreshToken.IsActive)
        {
            //Token không hoạt động
            throw new UserFriendlyException(ErrorCode.BadRequest, "Token not active");
        }

        //Thu hồi Token hiện tại
        refreshToken.Revoked = DateTime.UtcNow;

        //Tạo Token mới và cập nhật lại trên DB
        var res = await GenerateUserTokenAsync(user, null);

        var result = new TokenResult
        {
            UserId = res.UserId,
            Expires = res.Expires,
            Token = res.Token,
        };
        return result;
    }

    public async Task<TokenResult> Authenticate(LoginRequest request)
    {
        try
        {
            var user = (await _userManager.FindByNameAsync(request.UserName)
                        ?? await _userManager.FindByEmailAsync(request.UserName))
                        ?? throw new UserFriendlyException(ErrorCode.BadRequest, "Account Does Not Exist");

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);

            if (!result.Succeeded)
            {
                throw new UserFriendlyException(ErrorCode.BadRequest, "Login Unsuccessful");
            }

            var avatar = _context.Media.Where(m => m.MediaId == user.AvatarId)
                                       .Select(m => m.PathMedia)
                                       .FirstOrDefault();

            var token = await GenerateUserTokenAsync(user, avatar);

            return token;
        }

        catch (Exception exception)
        {
            if (exception is UserFriendlyException)
                throw;
            throw new UserFriendlyException(ErrorCode.Internal, "System error", exception);
        }
    }

    public async Task Register(RegisterRequest request)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user != null)
            {
                throw new UserFriendlyException(ErrorCode.NotFound, "Username is available");
            }

            user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                throw new UserFriendlyException(ErrorCode.NotFound, "Email is available");
            }

            user = new ApplicationUser()
            {
                Email = request.Email,
                UserName = request.UserName,
                Name = request.Name,
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Subscriber");
            }

            else
            {
                List<IdentityError> errorList = result.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));
                throw new UserFriendlyException(ErrorCode.BadRequest, errors, "Register unsuccessful");
            }
        }

        catch (Exception e)
        {
            throw new UserFriendlyException(ErrorCode.Internal, "System Error", e);
        }
    }

    public async Task<List<UserViewModel>> Get()
    {
        try
        {
            var users = await _userManager.Users.Include(x => x.UserRoles).ToListAsync();

            var query = users
                .Select(async u => new UserQueryResult
                {
                    users = u,
                    role = u.UserRoles
                        .Join(_roleManager.Roles,
                            userRole => userRole.RoleId,
                            role => role.Id,
                            (userRole, role) => role.Name)
                        .ToList(),
                    avatar = await _context.Media
                        .Where(m => m.MediaId == u.AvatarId)
                        .Select(m => m.PathMedia)
                        .FirstOrDefaultAsync(),
                })
                .ToList();

            var data = _mapper.Map<List<UserViewModel>>(query);

            data.ForEach(x => x.Avatar = !string.IsNullOrEmpty(x.Avatar)
                            ? _storageService.GetFileUrl(x.Avatar)
                            : x.Avatar);
            return data;
        }
        catch (Exception exception)
        {
            throw new UserFriendlyException(ErrorCode.Internal, "Failed to retrieve list user", exception);
        }
    }

    public async Task<bool> Update(UserUpdateRequest request)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.UserId)
                ?? throw new UserFriendlyException(ErrorCode.NotFound, "User not exist");

            user.Email = request.Email ?? user.Email;
            user.Name = request.Name ?? user.Name;

            //Lưu Avatar vào Host
            if (request.MediaFile != null)
            {
                var thumb = _context.Media.FirstOrDefault(i => i.MediaId == user.AvatarId);

                //Thêm mới Avatar nếu Tài khoản chưa có
                if (thumb == null)
                {
                    user.Avatar = new Media()
                    {
                        Caption = "Avatar User",
                        DateCreated = DateTime.Now,
                        FileSize = request.MediaFile.Length,
                        PathMedia = this.SaveFile(request.MediaFile),
                        Type = MediaType.Image,
                        SortOrder = 1
                    };
                }
                else
                {
                    //Cập nhật Avatar
                    if (thumb.PathMedia != null)
                    {
                        _storageService.DeleteFile(thumb.PathMedia);
                    }

                    thumb.FileSize = request.MediaFile.Length;
                    thumb.PathMedia = SaveFile(request.MediaFile);

                    _context.Media.Update(thumb);
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return false;
            }

            throw new UserFriendlyException(ErrorCode.BadRequest, "Update unsuccessful");
        }
        catch (Exception e)
        {
            throw new UserFriendlyException(ErrorCode.Internal, "Error system", e);
        }
    }

    //Lấy Id của người dùng
    public async Task<UserViewModel> GetById(Guid? id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString())
                    ?? throw new UserFriendlyException(ErrorCode.NotFound, "Account does not exist");
            var roles = await _userManager.GetRolesAsync(user);

            var avatar = _context.Media.Where(m => m.MediaId == user.AvatarId).Select(m => m.PathMedia)
                .FirstOrDefault();

            var userVm = _mapper.Map<UserViewModel>(user);

            userVm.Roles = roles;
            userVm.Avatar = avatar;
            return userVm;
        }

        catch (Exception e)
        {
            throw new UserFriendlyException(ErrorCode.Internal, "Error system", e);
        }
    }

    //Xoá người dùng
    public async Task<bool> Delete(string UserId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                throw new UserFriendlyException(ErrorCode.NotFound, "Account does not exist");
            }

            //Xoá Avatar ra khỏi Source
            var avatar = _context.Media.SingleOrDefault(x => x.MediaId == user.AvatarId);

            if (avatar != null)
            {
                if (avatar.PathMedia != null)
                    _storageService.DeleteFile(avatar.PathMedia);
                _context.Media.Remove(avatar);
            }

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded ? false : throw new UserFriendlyException(ErrorCode.BadRequest, "Delete Unsuccessful");
        }
        catch (Exception e)
        {
            throw new UserFriendlyException(ErrorCode.Internal, "Error system", e);
        }
    }

    //Gán quyền người dùng
    public async Task<bool> RoleAssign(Guid id, RoleAssignRequest request)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            throw new UserFriendlyException(ErrorCode.NotFound, "Account does not exist");
        }

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

        return false;
    }

    //Lưu ảnh
    private string SaveFile(IFormFile file)
    {
        var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
        _storageService.SaveFile(file.OpenReadStream(), fileName);
        return fileName;
    }

    //Đăng nhập bằng Facebook
    public async Task<TokenResult> SignInFacebook(string accessToken)
    {
        try
        {
            var validatedTokenResult = await _facebookAuthService.ValidationAcessTokenAsync(accessToken);

            if (!validatedTokenResult.Data.IsValid)
            {
                throw new UserFriendlyException(ErrorCode.NotFound, "Invalid Facebook Token");
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

                var tokenResult = await GenerateUserTokenAsync(exist_user, avatar);

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

                        var tokenResult = await GenerateUserTokenAsync(exist_user, avatar);

                        return tokenResult;
                    }
                    else
                    {
                        throw new UserFriendlyException(ErrorCode.BadRequest, "Error Linked Facebook");
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
                            throw new UserFriendlyException(ErrorCode.BadRequest, "Error Linked Facebook");
                        }

                        var roles = await _userManager.AddToRoleAsync(indentityuser, "Subscriber");


                        var tokenResult = await GenerateUserTokenAsync(exist_user, null);
                        return tokenResult;
                    }
                    else
                    {
                        List<IdentityError> errorList = result.Errors.ToList();
                        var errors = string.Join(", ", errorList.Select(e => e.Description));
                        throw new UserFriendlyException(ErrorCode.BadRequest, errors, "Register Facebook Unsuccessful");
                    }
                }
            }
        }
        catch (Exception e)
        {
            throw new UserFriendlyException(ErrorCode.Internal, "Error system", e);

        }
    }

    //Đăng nhập bằng Google
    public async Task<TokenResult> SignInGoogle(string accessToken)
    {
        try
        {
            Payload payload = await ValidateAsync(accessToken) 
                ?? throw new UserFriendlyException(ErrorCode.NotFound, "Invalid Google token");

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

                var tokenResult = await GenerateUserTokenAsync(exist_user, avatar);

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

                        var tokenResult = await GenerateUserTokenAsync(exist_user, avatar);

                        return tokenResult;
                    }
                    else
                    {
                        throw new UserFriendlyException(ErrorCode.BadRequest, "Error Linked Google");

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
                            throw new UserFriendlyException(ErrorCode.BadRequest, "Error Linked Google");

                        }

                        var roles = await _userManager.AddToRoleAsync(indentityuser, "Subscriber");


                        var tokenResult = await GenerateUserTokenAsync(exist_user, null);

                        return tokenResult;
                    }
                    else
                    {
                        List<IdentityError> errorList = result.Errors.ToList();
                        var errors = string.Join(", ", errorList.Select(e => e.Description));
                        throw new UserFriendlyException(ErrorCode.BadRequest, errors, "RegisterGoogleUnsuccessful");

                    }
                }
            }
        }
        catch (Exception e)
        {
            throw new UserFriendlyException(ErrorCode.Internal, "Error system", e);

        }
    }

    //Đăng nhập bằng Apple
    public async Task<TokenResult> SignInApple(string fullName, string identityToken)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var payload = tokenHandler.ReadJwtToken(identityToken);

            if (payload == null)
            {
                throw new UserFriendlyException(ErrorCode.NotFound, "Invalid Apple Token");

            }

            //Kiểm tra user đã liên kết với Google chưa
            var checkLinked = await _signInManager.ExternalLoginSignInAsync("Apple", payload.Subject, false);

            //Cắt Email thành UserName
            string email = payload.Payload.SingleOrDefault(x => x.Key == "email").Value.ToString();

            if (email == null)
            {
                throw new UserFriendlyException(ErrorCode.NotFound, "Email is required");
            }

            string userName = email.Split('@')[0];

            //Nếu đã có liên kết trước đó (Email) thì tiến hành đăng nhập luôn
            if (checkLinked.Succeeded)
            {
                var exist_user = await _userManager.FindByEmailAsync(email);

                var avatar = _context.Media.Where(m => m.MediaId == exist_user.AvatarId).Select(m => m.PathMedia)
                    .FirstOrDefault();

                var tokenResult = await GenerateUserTokenAsync(exist_user, avatar);

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

                        var tokenResult = await GenerateUserTokenAsync(exist_user, avatar);

                        return tokenResult;
                    }
                    else
                    {
                        throw new UserFriendlyException(ErrorCode.BadRequest, "Error linked apple");
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
                            throw new UserFriendlyException(ErrorCode.BadRequest, "Error linked apple");

                        }

                        var roles = await _userManager.AddToRoleAsync(indentityuser, "Subscriber");


                        var tokenResult = await GenerateUserTokenAsync(exist_user, null);

                        return tokenResult;
                    }
                    else
                    {
                        List<IdentityError> errorList = result.Errors.ToList();
                        var errors = string.Join(", ", errorList.Select(e => e.Description));
                        throw new UserFriendlyException(ErrorCode.BadRequest, errors, "Register Apple Unsuccessful");
                    }
                }
            }

        }
        catch (Exception e)
        {
            throw new UserFriendlyException(ErrorCode.Internal, "Error system", e);

        }
    }

    public async Task<ForgotPassword> SendPasswordResetCode(string email)
    {
        //Get identity user details user manager
        var user = await _userManager.FindByEmailAsync(email) ?? throw new UserFriendlyException(ErrorCode.NotFound, "User Not Found");

        //Generate password reset token
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        //Generate OTP
        int otp = StringHelper.GenerateRandom(100000, 999999);

        var resetPassword = new ForgotPassword()
        {
            Email = email,
            OTP = otp.ToString(),
            Token = token,
            UserId = user.Id,
            DateTime = DateTime.Now
        };

        //save data into db with OTP
        _context.ForgotPassword.Add(resetPassword);
        await _context.SaveChangesAsync();

        //To do: Send token in email
        await _emailSender.SendEmailAsync(email, "Reset Password OTP", "Hello "
            + email +
            "<br><br>Please find the reset password token below<br><br><b>"
            + otp +
            "<b><br><br>Thanks<br>truongnhon.tk");

        return resetPassword;
    }

    public async Task<ForgotPassword> ResetPassword(string email, string otp, string newPassword)
    {
        //Get identity user details user manager
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null || string.IsNullOrEmpty(newPassword))
        {
            throw new UserFriendlyException(ErrorCode.BadRequest, "EmailAndPasswordNotNull");
        }

        //Getting token from otp
        var resetPasswordDetails = await _context.ForgotPassword
            .Where(x => x.OTP == otp && x.UserId == user.Id)
            .OrderByDescending(x => x.DateTime).FirstOrDefaultAsync();

        //Verify if token is older than 3 minutes
        var expirationDateTime = resetPasswordDetails.DateTime.AddMinutes(3);

        if (expirationDateTime < DateTime.Now)
        {
            throw new UserFriendlyException(ErrorCode.BadRequest, "GenerateTheNewOTP");
        }

        var res = await _userManager.ResetPasswordAsync(user, resetPasswordDetails.Token, newPassword);

        return !res.Succeeded ? throw new UserFriendlyException(ErrorCode.BadRequest, "OTPWrong") : new ForgotPassword();
    }

    public async Task<UserViewModel> Profile()
    {
        return await GetById(_user.GetCurrentGuidUserId());
    }
}
