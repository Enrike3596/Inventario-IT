namespace Services.Emails
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage message);
        Task SendAsync(string to, string subject, string body);
    }
}
