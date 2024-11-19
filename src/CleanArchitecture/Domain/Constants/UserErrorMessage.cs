namespace CleanArchitecture.Domain.Constants;

public static class UserErrorMessage
{
    public const string AlreadyExists = "{0} already exists!";
    public const string Unauthorized = "User is not logged in.";
    public const string UserNotExist = "The specified user does not exist.";
    public const string PasswordIncorrect = "The password entered is incorrect.";
}

