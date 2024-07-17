namespace CleanArchitecture.Application.Common.Interfaces;

public interface IMailService
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}
