using System.Diagnostics.CodeAnalysis;
using CleanArchitecture.Domain.Constants;

namespace CleanArchitecture.Application.Common.Exceptions;

[ExcludeFromCodeCoverage]
public static class ProgramException
{
    public static UserFriendlyException AppsettingNotSetException()
        => new(ErrorCode.Internal, ErrorMessage.AppConfigurationMessage, ErrorMessage.InternalError);
}
