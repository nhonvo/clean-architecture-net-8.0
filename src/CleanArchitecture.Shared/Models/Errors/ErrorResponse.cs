namespace CleanArchitecture.Shared.Models.Errors;

public class ErrorResponse
{
    public IEnumerable<Error> Errors { get; set; } = [];
    public ErrorResponse()
    {

    }
    public ErrorResponse(IEnumerable<Error> errors)
    {
        Errors = errors;
    }
}
