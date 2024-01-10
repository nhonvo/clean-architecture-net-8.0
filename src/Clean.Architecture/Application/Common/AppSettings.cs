#nullable disable
namespace Clean.Architecture.Application.Common
{
    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
        public string SqlServerConnection { get; set; }
    }

    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }

    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public Jwt Jwt { get; set; }
        public bool UseInMemoryDatabase { get; set; }
        public bool UseDocker { get; set; }
        public Logging Logging { get; set; }
    }

    public class Logging
    {
        public RequestResponse RequestResponse { get; set; }
    }

    public class RequestResponse
    {
        public bool IsEnabled { get; set; }
    }
}
