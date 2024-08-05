namespace CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;
public class ResetPasswordRequest
{
    public string Email { get; set; }
    public string OTP { get; set; }
    public string NewPassword { get; set; }

}
