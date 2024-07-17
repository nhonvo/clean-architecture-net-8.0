namespace CleanArchitecture.Application.Common.Exceptions;

public class UserFriendlyException : Exception
{
    public string UserFriendlyMessage { get; set; }
    public ErrorCode ErrorCode { get; set; }
    
    public UserFriendlyException(ErrorCode errorCode, string userFriendlyMessage, Exception? innerException = null) : base(userFriendlyMessage, innerException)
    {
        ErrorCode = errorCode;
        UserFriendlyMessage = userFriendlyMessage;
    }
    public UserFriendlyException(string message, string userFriendlyMessage, Exception? innerException = null) : base(message, innerException)
    {
        UserFriendlyMessage = userFriendlyMessage;
    }
    public UserFriendlyException(ErrorCode errorCode, string message, string userFriendlyMessage, Exception? innerException = null) : base(message, innerException)
    {
        ErrorCode = errorCode;
        UserFriendlyMessage = userFriendlyMessage;
    }
}
