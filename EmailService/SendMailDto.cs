namespace EmailService
{
    public class SendMailDto
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public string ToMails { get; set; }   // Virgülle ayrılarak birden fazla mail girilebilir
        public string CC { get; set; }
        public List<IFormFile> Attachements { get; set; }
    }
}
