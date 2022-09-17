namespace EmailService
{
    public interface IEmailProvider
    {
        void SendEmail(Message message);
    }
}
