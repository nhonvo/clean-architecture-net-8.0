using CleanArchitecture.Shared.Models.Errors;

namespace CleanArchitecture.Application.Common.Exceptions;

public class ValidationException(ErrorResponse errorResponse) : Exception
{
    public ErrorResponse ErrorResponse { get; private set; } = errorResponse;
}
