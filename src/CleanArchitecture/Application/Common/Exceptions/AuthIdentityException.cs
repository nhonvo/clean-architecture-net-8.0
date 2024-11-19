using System.Diagnostics.CodeAnalysis;
using CleanArchitecture.Domain.Constants;

namespace CleanArchitecture.Application.Common.Exceptions;

[ExcludeFromCodeCoverage]
public static class AuthIdentityException
{
    public static UserFriendlyException ThrowTokenNotExist()
        => throw new UserFriendlyException(ErrorCode.NotFound, AuthIdentityErrorMessage.TokenNotExistMessage, AuthIdentityErrorMessage.TokenNotExistMessage);

    public static UserFriendlyException ThrowTokenNotActive()
        => throw new UserFriendlyException(ErrorCode.BadRequest, AuthIdentityErrorMessage.TokenNotActiveMessage, AuthIdentityErrorMessage.TokenNotActiveMessage);

    public static UserFriendlyException ThrowAccountDoesNotExist()
        => throw new UserFriendlyException(ErrorCode.NotFound, AuthIdentityErrorMessage.AccountDoesNotExistMessage, AuthIdentityErrorMessage.AccountDoesNotExistMessage);

    public static UserFriendlyException ThrowLoginUnsuccessful()
        => throw new UserFriendlyException(ErrorCode.BadRequest, AuthIdentityErrorMessage.LoginUnsuccessfulMessage, AuthIdentityErrorMessage.LoginUnsuccessfulMessage);

    public static UserFriendlyException ThrowUsernameAvailable()
        => throw new UserFriendlyException(ErrorCode.NotFound, AuthIdentityErrorMessage.UsernameAvailableMessage, AuthIdentityErrorMessage.UsernameAvailableMessage);

    public static UserFriendlyException ThrowEmailAvailable()
        => throw new UserFriendlyException(ErrorCode.NotFound, AuthIdentityErrorMessage.EmailAvailableMessage, AuthIdentityErrorMessage.EmailAvailableMessage);

    public static UserFriendlyException ThrowRegisterUnsuccessful(string errors)
        => throw new UserFriendlyException(ErrorCode.BadRequest, AuthIdentityErrorMessage.RegisterUnsuccessfulMessage, errors);

    public static UserFriendlyException ThrowUserNotExist()
        => throw new UserFriendlyException(ErrorCode.NotFound, AuthIdentityErrorMessage.UserNotExistMessage, AuthIdentityErrorMessage.UserNotExistMessage);

    public static UserFriendlyException ThrowUpdateUnsuccessful(string errors)
        => throw new UserFriendlyException(ErrorCode.BadRequest, AuthIdentityErrorMessage.UpdateUnsuccessfulMessage, errors);

    public static UserFriendlyException ThrowDeleteUnsuccessful()
        => throw new UserFriendlyException(ErrorCode.BadRequest, AuthIdentityErrorMessage.DeleteUnsuccessfulMessage, AuthIdentityErrorMessage.DeleteUnsuccessfulMessage);

    public static UserFriendlyException ThrowInvalidFacebookToken()
        => throw new UserFriendlyException(ErrorCode.NotFound, AuthIdentityErrorMessage.InvalidFacebookTokenMessage, AuthIdentityErrorMessage.InvalidFacebookTokenMessage);

    public static UserFriendlyException ThrowErrorLinkedFacebook()
        => throw new UserFriendlyException(ErrorCode.BadRequest, AuthIdentityErrorMessage.ErrorLinkedFacebookMessage, AuthIdentityErrorMessage.ErrorLinkedFacebookMessage);

    public static UserFriendlyException ThrowRegisterFacebookUnsuccessful(string errors)
        => throw new UserFriendlyException(ErrorCode.BadRequest, AuthIdentityErrorMessage.RegisterFacebookUnsuccessfulMessage, errors);

    public static UserFriendlyException ThrowInvalidGoogleToken()
        => throw new UserFriendlyException(ErrorCode.NotFound, AuthIdentityErrorMessage.InvalidGoogleTokenMessage, AuthIdentityErrorMessage.InvalidGoogleTokenMessage);

    public static UserFriendlyException ThrowErrorLinkedGoogle()
        => throw new UserFriendlyException(ErrorCode.BadRequest, AuthIdentityErrorMessage.ErrorLinkedGoogleMessage, AuthIdentityErrorMessage.ErrorLinkedGoogleMessage);

    public static UserFriendlyException ThrowRegisterGoogleUnsuccessful(string errors)
        => throw new UserFriendlyException(ErrorCode.BadRequest, AuthIdentityErrorMessage.RegisterGoogleUnsuccessfulMessage, errors);

    public static UserFriendlyException ThrowInvalidAppleToken()
        => throw new UserFriendlyException(ErrorCode.NotFound, AuthIdentityErrorMessage.InvalidAppleTokenMessage, AuthIdentityErrorMessage.InvalidAppleTokenMessage);

    public static UserFriendlyException ThrowEmailRequired()
        => throw new UserFriendlyException(ErrorCode.NotFound, AuthIdentityErrorMessage.EmailRequiredMessage, AuthIdentityErrorMessage.EmailRequiredMessage);

    public static UserFriendlyException ThrowErrorLinkedApple()
        => throw new UserFriendlyException(ErrorCode.BadRequest, AuthIdentityErrorMessage.ErrorLinkedAppleMessage, AuthIdentityErrorMessage.ErrorLinkedAppleMessage);

    public static UserFriendlyException ThrowRegisterAppleUnsuccessful(string errors)
        => throw new UserFriendlyException(ErrorCode.BadRequest, AuthIdentityErrorMessage.RegisterAppleUnsuccessfulMessage, errors);

    public static UserFriendlyException ThrowUserNotFound()
        => throw new UserFriendlyException(ErrorCode.NotFound, AuthIdentityErrorMessage.UserNotFoundMessage, AuthIdentityErrorMessage.UserNotFoundMessage);

    public static UserFriendlyException ThrowGenerateTheNewOTP()
        => throw new UserFriendlyException(ErrorCode.BadRequest, AuthIdentityErrorMessage.GenerateTheNewOTPMessage, AuthIdentityErrorMessage.GenerateTheNewOTPMessage);

    public static UserFriendlyException ThrowOTPWrong()
        => throw new UserFriendlyException(ErrorCode.BadRequest, AuthIdentityErrorMessage.OTPWrongMessage, AuthIdentityErrorMessage.OTPWrongMessage);
}
