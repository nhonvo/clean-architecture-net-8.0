namespace CleanArchitecture.Application.Common.Utilities;

public static class StringHelper
{
    public static string Hash(this string inputString)
        => BCrypt.Net.BCrypt.HashPassword(inputString);

    public static bool Verify(string Pass, string oldPass)
        => BCrypt.Net.BCrypt.Verify(Pass, oldPass);
}
