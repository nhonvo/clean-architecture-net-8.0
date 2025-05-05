namespace CleanArchitecture.Jobs;

public class EmailService
{
    public void SendEmailReport()
    {
        Console.WriteLine($"Email sent at {DateTime.Now}");
    }
}