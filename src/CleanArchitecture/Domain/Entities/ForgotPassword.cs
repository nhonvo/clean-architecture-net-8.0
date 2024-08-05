namespace CleanArchitecture.Domain.Entities;

public class ForgotPassword
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public string OTP { get; set; }
    public DateTime DateTime { get; set; }
}
