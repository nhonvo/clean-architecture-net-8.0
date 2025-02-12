#nullable disable

using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Application.Common;

public class AppSettings
{
    public string AppUrl { get; set; }
    public ApplicationDetail ApplicationDetail { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public Identity Identity { get; set; }
    public MailConfigurations MailConfigurations { get; set; }
    public FileStorageSettings FileStorageSettings { get; set; }
    public CloudinarySettings Cloudinary { get; set; }
    public bool UseInMemoryDatabase { get; set; }
    public string[] Cors { get; set; }
    public string BaseURL { get; set; }
    public bool EnableExternalHealthCheck { get; set; }
}

public class ApplicationDetail
{
    public string ApplicationName { get; set; }
    public string Description { get; set; }
    public string ContactWebsite { get; set; }
}

public class ConnectionStrings
{
    [Required]
    public string DefaultConnection { get; set; }
}

public class Identity
{
    [Required]
    public bool IsLocal { get; set; } = false;
    [Required]
    public string Key { get; set; }
    [Required]
    public string Issuer { get; set; }
    [Required]
    public string Audience { get; set; }
    [Required]
    public string ScopeBaseDomain { get; set; }
    [Required]
    public bool ValidateHttps { get; set; }
    public int ExpiredTime { get; set; } = 10;
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
public class FileStorageSettings
{
    public bool LocalStorage { get; set; } = true;
    [Required]
    public string Path { get; set; }
}
