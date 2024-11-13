using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Shared.Models.AuthIdentity.UsersIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Application.Common.Utilities;

public class TokenService(AppSettings appSettings,
    ICurrentTime time,
    IUnitOfWork unitOfWork,
    UserManager<ApplicationUser> userManager) : ITokenService
{
    private readonly AppSettings _appSettings = appSettings;
    private readonly Identity _jwt = appSettings.Identity;
    private readonly ICurrentTime _time = time;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Identity.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var token = new JwtSecurityToken(
                claims: claims,
                expires: _time.GetCurrentTime().AddDays(1),
                audience: _appSettings.Identity.Audience,
                issuer: _appSettings.Identity.Issuer,
                signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        IdentityModelEventSource.ShowPII = true;
        TokenValidationParameters validationParameters = new()
        {
            ValidIssuer = _appSettings.Identity.Issuer,
            ValidAudience = _appSettings.Identity.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Identity.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };

        var principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);

        return principal;
    }

    public async Task<TokenResult> GenerateToken(ApplicationUser user, string[] scopes, CancellationToken cancellationToken)
    {
        var result = new TokenResult();

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.GivenName, user.Name),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Uri, user?.Avatar?.PathMedia ?? "default.png"),
            new Claim(ClaimTypes.Role, roles == null ? Role.User.ToString() : string.Join(";", roles)),
            new Claim("scope", string.Join(" ", scopes)) // Adding scope claim
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddDays(_jwt.ExpiredTime);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);

        result.UserId = user.Id;
        result.Expires = expires;
        result.Token = tokenResult;

        var refreshToken = new RefreshToken
        {
            Token = tokenResult,
            UserId = user.Id,
            Expires = expires,
            Created = DateTime.UtcNow
        };
        var checkToken = await _unitOfWork.RefreshTokenRepository.FirstOrDefaultAsync(r => r.UserId == user.Id);

        //if refresh token is not exist, then add new one
        if (checkToken == null)
        {
            await _unitOfWork.ExecuteTransactionAsync(
                async () => await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken), cancellationToken);
        }
        //if refresh token is exist and valid, then update it
        else if (checkToken.Expires > DateTime.UtcNow)
        {
            checkToken.Token = tokenResult;
            checkToken.Expires = expires;
            checkToken.Created = DateTime.UtcNow;
            await _unitOfWork.ExecuteTransactionAsync(
                () => _unitOfWork.RefreshTokenRepository.Update(refreshToken), cancellationToken);

        }
        //if refresh token is exist and expired, then delete it and add new one
        else
        {
            await _unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    _unitOfWork.RefreshTokenRepository.Delete(refreshToken);
                    await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
                }, cancellationToken);
        }
        return result;
    }
}
