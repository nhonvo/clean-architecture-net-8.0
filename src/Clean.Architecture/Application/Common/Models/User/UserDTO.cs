namespace Clean.Architecture.Application.Common.Models.User;

public class UserDTO
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? Token { get; set; }
}
