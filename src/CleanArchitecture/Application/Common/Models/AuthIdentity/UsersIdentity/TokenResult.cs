namespace CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;

public class TokenResult
{
    public Guid UserId { get; set; }

    public string Token { get; set; }

    public DateTime Expires { get; set; }
}
