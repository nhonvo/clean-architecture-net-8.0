using System.Text;
using Hangfire.Dashboard;

namespace CleanArchitecture.Jobs;

public class HangfireBasicAuthenticationFilterDevelopment() : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}
public class HangfireBasicAuthenticationFilter(string username, string password) : IDashboardAuthorizationFilter
{
    private readonly string _username = username;
    private readonly string _password = password;

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        string authHeader = httpContext.Request.Headers["Authorization"];
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic "))
        {
            Challenge(httpContext);
            return false;
        }

        string encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
        string decodedCredentials;

        try
        {
            var credentialBytes = Convert.FromBase64String(encodedCredentials);
            decodedCredentials = Encoding.UTF8.GetString(credentialBytes);
        }
        catch
        {
            Challenge(httpContext);
            return false;
        }

        var parts = decodedCredentials.Split(':');
        if (parts.Length != 2)
        {
            Challenge(httpContext);
            return false;
        }

        var username = parts[0];
        var password = parts[1];

        if (username == _username && password == _password)
        {
            return true;
        }

        Challenge(httpContext);
        return false;
    }

    private void Challenge(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = 401;
        httpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Hangfire Dashboard\"";
    }
}