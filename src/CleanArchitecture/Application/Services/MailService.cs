using System.Net;
using System.Net.Mail;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Services;

public class MailService(AppSettings appSettings) : IMailService
{
    private readonly MailConfigurations _mailSettings = appSettings.MailConfigurations;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        string fromMail = _mailSettings.From;
        string fromPassword = _mailSettings.Password;

        MailMessage message = new()
        {
            From = new MailAddress(fromMail),
            Subject = subject
        };
        message.To.Add(new MailAddress(email));
        message.Body = "<html><body>" + htmlMessage + "</body></html>";
        message.IsBodyHtml = true;

        var smtpClient = new SmtpClient(_mailSettings.Host)
        {
            Port = _mailSettings.Port,
            Credentials = new NetworkCredential(fromMail, fromPassword),
            EnableSsl = true,
        };

        await smtpClient.SendMailAsync(message);
    }
}
