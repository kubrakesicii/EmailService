using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EmailService
{
    public class EmailProvider : IEmailProvider
    {
        public readonly EmailConfiguration _emailConfig;
        public IConfiguration Configuration { get; }

        public EmailProvider(EmailConfiguration emailConfig, IConfiguration configuration)
        {
            _emailConfig = emailConfig;
            Configuration = configuration;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.Cc.Add(new MailboxAddress(_emailConfig.CC,""));
            emailMessage.From.Add(new MailboxAddress(_emailConfig.DisplayName, _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var builder = new BodyBuilder();
            if (message.Attachments != null && message.Attachments.Count > 0)
            {
                message.Attachments.ForEach(a =>
                {
                    byte[] fileBytes;

                    using (var ms = new MemoryStream())
                    {
                        a.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    builder.Attachments.Add(a.FileName, fileBytes, ContentType.Parse(a.ContentType));
                });
            }

            builder.HtmlBody = message.Content;
            emailMessage.Body = builder.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.Username, Configuration["EMAIL_PASS"]);

                    client.Send(mailMessage);
                }

                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
