using CleanArchitecture.Domain.Constants;

namespace CleanArchitecture.Application.Common.Exceptions;

public static class ProgramException
{
    public static UserFriendlyException AppsettingNotSetException()
        => new(ErrorCode.Internal, ErrorMessage.AppConfigurationMessage, ErrorMessage.Internal);
}
