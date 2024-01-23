namespace CleanArchitecture.Domain
{
    public static class Constant
    {
        public static class Application
        {
            public static string Name = "CleanArchitecture";
        }
        public static class Url
        {
            public static string BookData = @"../../Solution Items/data/book.json";
            public static string UserData = @"../../Solution Items/data/user.json";
        }
        public static class ErrorMessage
        {
            public static string NotFoundMessage = "Could not find";
            public static string AppConfigurationMessage = "AppConfiguration cannot be null";
        }
        public static class SeedingMessage
        {
            public static string SeedDataSuccessMessage = "Seed data successfully";
        }
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
    }
}
