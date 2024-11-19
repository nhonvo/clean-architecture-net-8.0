namespace CleanArchitecture.Domain.Constants;

public static class AuthIdentityErrorMessage
{
    public const string TokenNotExistMessage = "The specified token does not exist.";
    public const string TokenNotActiveMessage = "The specified token is not active.";
    public const string AccountDoesNotExistMessage = "The account does not exist.";
    public const string LoginUnsuccessfulMessage = "Login was unsuccessful. Check you password again!";
    public const string RefreshTokenUnsuccessfulMessage = "Refresh token was unsuccessful.";
    public const string UsernameAvailableMessage = "The username is available.";
    public const string EmailAvailableMessage = "The email address is available.";
    public const string RegisterUnsuccessfulMessage = "Registration was unsuccessful.";
    public const string UserNotExistMessage = "The user does not exist.";
    public const string UpdateUnsuccessfulMessage = "The update operation was unsuccessful.";
    public const string DeleteUnsuccessfulMessage = "The delete operation was unsuccessful.";
    public const string InvalidFacebookTokenMessage = "The provided Facebook token is invalid.";
    public const string ErrorLinkedFacebookMessage = "An error occurred while linking the Facebook account.";
    public const string RegisterFacebookUnsuccessfulMessage = "Registration via Facebook was unsuccessful.";
    public const string InvalidGoogleTokenMessage = "The provided Google token is invalid.";
    public const string ErrorLinkedGoogleMessage = "An error occurred while linking the Google account.";
    public const string RegisterGoogleUnsuccessfulMessage = "Registration via Google was unsuccessful.";
    public const string InvalidAppleTokenMessage = "The provided Apple token is invalid.";
    public const string EmailRequiredMessage = "An email address is required.";
    public const string ErrorLinkedAppleMessage = "An error occurred while linking the Apple account.";
    public const string RegisterAppleUnsuccessfulMessage = "Registration via Apple was unsuccessful.";
    public const string UserNotFoundMessage = "The user was not found.";
    public const string EmailAndPasswordNotNullMessage = "Email and password must not be null.";
    public const string GenerateTheNewOTPMessage = "A new OTP needs to be generated.";
    public const string OTPWrongMessage = "The OTP provided is incorrect.";

}
