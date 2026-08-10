using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Services.Emails
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;

        public SmtpEmailSender(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendAsync(EmailMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            mimeMessage.To.Add(MailboxAddress.Parse(message.To));
            mimeMessage.Subject = message.Subject;

            var body = new TextPart(message.IsHtml ? "html" : "plain")
            {
                Text = message.Body
            };

            if (message.AttachmentPaths is { Count: > 0 })
            {
                var multipart = new Multipart("mixed") { body };
                foreach (var path in message.AttachmentPaths)
                {
                    if (File.Exists(path))
                        multipart.Add(new MimePart { Content = new MimeContent(File.OpenRead(path)), FileName = Path.GetFileName(path) });
                }
                mimeMessage.Body = multipart;
            }
            else
            {
                mimeMessage.Body = body;
            }

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, _settings.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto);
            await client.AuthenticateAsync(_settings.Username, _settings.Password);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }

        public Task SendAsync(string to, string subject, string body)
        {
            return SendAsync(new EmailMessage
            {
                To = to,
                Subject = subject,
                Body = body,
                IsHtml = true
            });
        }
    }
}
