using MimeKit;

namespace EmailService
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public List<IFormFile> Attachments { get; set; }
        public Message(IEnumerable<string> to, string cc, string subject, string content, List<IFormFile> attachments)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x,x)));

            CC = cc;
            Subject = subject;
            Content = content;
            Attachments = attachments;
        }
    }
}
