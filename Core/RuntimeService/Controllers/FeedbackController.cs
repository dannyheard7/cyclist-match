using System;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using RuntimeService.Services;

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
    [Route("api/feedback")]
    public class FeedbackController : ControllerBase
    {
        private readonly MailOptions _mailOptions;
        private readonly IUserContext _userContext;
        
        public FeedbackController(IUserContext userContext, IConfiguration configuration)
        {
            _userContext = userContext;
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
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Feedback form submitted");
            stringBuilder.AppendLine(input.Message);

            if (input.Email != null)
            {
                stringBuilder.AppendLine(input.Email);

            }

            try
            {
                var profile = await _userContext.GetUserProfile();

                if (profile != null)
                {
                    stringBuilder.AppendLine($"User - Id: {profile.UserId}, Name: {profile.UserDisplayName}");
                }
            }
            catch (Exception)
            {
                
            }
            
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_mailOptions.UserName));
            email.To.Add(MailboxAddress.Parse(_mailOptions.FeedbackEmail));
            email.Subject = "Feedback Submitted";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = stringBuilder.ToString()
            };
            
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailOptions.Host, _mailOptions.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailOptions.UserName, _mailOptions.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            return NoContent();
        }
    }
}