using System;
using System.Threading.Tasks;
using Auth;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace RuntimeService.Controllers
{
    public class MailOptions
    {
        public static string Mail = "Mail";
        
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        
        public string FeedbackEmail { get; set; }
    }
    
    [ApiController]
    [Route("feedback")]
    public class FeedbackController : ControllerBase
    {
        private readonly MailOptions _mailOptions;
        private readonly ICurrentUserService _currentUserService;
        
        public FeedbackController(ICurrentUserService currentUserService, IConfiguration configuration)
        {
            _currentUserService = currentUserService;
            _mailOptions = configuration.GetSection(MailOptions.Mail).Get<MailOptions>();
        }
        
        public class FeedbackInput
        {
            public string? Email { get; set; }
            public string Message { get; set; }
        }
        
        [HttpPost]
        public async Task<IActionResult> PostFeedback([FromBody] FeedbackInput input)
        {
            var emailAddress = input.Email;

            try
            {
                var user = await _currentUserService.GetUser();
                emailAddress = user.Email;
            } catch(UnauthorizedAccessException){}
            
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_mailOptions.UserName));
            email.To.Add(MailboxAddress.Parse(_mailOptions.FeedbackEmail));
            email.Subject = "Feedback Submitted";
            email.Body = new TextPart(TextFormat.Html) { Text = $@"{emailAddress} - {input.Message}"};
            
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailOptions.Host, _mailOptions.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailOptions.UserName, _mailOptions.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            return NoContent();
        }
    }
}