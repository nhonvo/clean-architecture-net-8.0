#nullable disable

namespace CleanArchitecture.Application.Common;

public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
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
    public Logging Logging { get; set; }
    public string[] Cors { get; set; }
    public ApplicationDetail ApplicationDetail { get; set; }
    public MailConfigurations MailConfigurations { get; set; }
}

public class MailConfigurations
{
    public string From { get; set; }
    public string Host { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
}

public class ApplicationDetail
{
    public string ApplicationName { get; set; }
    public string Description { get; set; }
    public string ContactWebsite { get; set; }
    public string LicenseDetail { get; set; }
}

public class Logging
{
    public RequestResponse RequestResponse { get; set; }
}

public class RequestResponse
{
    public bool IsEnabled { get; set; }
}
