using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace EmailService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailProvider _emailProvider;

        public EmailsController(IEmailProvider emailProvider)
        {
            _emailProvider = emailProvider;
        }

        [HttpPost("Send")]
        public IActionResult SendMail([FromForm] SendMailDto mail)
        {
            var toMails = mail.ToMails != null ? mail.ToMails.Split(",").ToList() : new List<string> { };
            var message = new Message(toMails, mail.CC, mail.Subject, mail.Content, mail.Attachements);
            _emailProvider.SendEmail(message);
            return Ok();
        }
    }
}
