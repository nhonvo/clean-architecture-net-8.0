namespace CleanArchitecture.Domain.Constants;

public static class ErrorRespondCode
{
    public const string NOT_FOUND = "not_found";
    public const string VERSION_CONFLICT = "version_conflict";
    public const string ITEM_ALREADY_EXISTS = "item_exists";
    public const string CONFLICT = "conflict";
    public const string BAD_REQUEST = "bad_request";
    public const string UNAUTHORIZED = "unauthorized";
    public const string INTERNAL_ERROR = "internal_error";
    public const string GENERAL_ERROR = "general_error";
    public const string UNPROCESSABLE_ENTITY = "unprocessable_entity";
}
