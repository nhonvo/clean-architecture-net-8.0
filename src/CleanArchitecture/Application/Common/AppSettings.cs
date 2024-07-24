#nullable disable

using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Application.Common;

public class ConnectionStrings
{
    [Required]
    public string DefaultConnection { get; set; }
}

public class Jwt
{
    [Required]
    public string Key { get; set; }
    [Required]
    public string Issuer { get; set; }
    [Required]
    public string Audience { get; set; }
    public int ExpiredTime { get; set; } = 10;
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
    public CloudinarySettings Cloudinary { get; set; }
}

public class MailConfigurations
{
    [Required]
    public string From { get; set; }
    [Required]
    public string Host { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public int Port { get; set; }
}

public class ApplicationDetail
{

    [Required]
    public string ApplicationName { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string ContactWebsite { get; set; }
    [Required]
    public string LicenseDetail { get; set; }
}

public class Logging
{
    public RequestResponse RequestResponse { get; set; }
}

public class RequestResponse
{
    public bool IsEnabled { get; set; } = true;
}


public class CloudinarySettings
{
    [Required]
    public string CloudName { get; set; } = string.Empty;
    [Required]
    public string ApiKey { get; set; } = string.Empty;
    [Required]
    public string ApiSecret { get; set; } = string.Empty;
}
