using CleanArchitecture.Domain.Constants;

namespace CleanArchitecture.Application.Common.Exceptions
{
    public static class UserException
    {
        public static UserFriendlyException UserAlreadyExistsException(string field)
            => new(ErrorCode.BadRequest, string.Format(UserErrorMessage.AlreadyExists, field));

        public static UserFriendlyException UnauthorizedException()
            => new(ErrorCode.Unauthorized, UserErrorMessage.Unauthorized);

        public static UserFriendlyException InternalException(Exception? exception)
            => new(ErrorCode.Internal, ErrorMessage.Internal, exception.Message);

        public static UserFriendlyException BadRequestException(string errorMessage)
            => new(ErrorCode.BadRequest, errorMessage);

    }
}
