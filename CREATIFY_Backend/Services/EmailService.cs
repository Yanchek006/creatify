// Services/EmailService.cs
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CREATIFY_Backend.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task SendNotificationEmail(string subject, string body)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            
            using (var client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings["FromEmail"], "CREATIFY"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                
                mailMessage.To.Add("creatify_cf@mail.ru");
                
                await client.SendMailAsync(mailMessage);
            }
        }
        
        public async Task SendAutoReply(string toEmail, string name, string formType)
        {
            string subject = "Спасибо за обращение в CREATIFY!";
            string body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2 style='color: #4A6CF7;'>Здравствуйте, {name}!</h2>
                    <p>Спасибо за ваше обращение в CREATIFY Digital Design.</p>
                    <p>Мы получили вашу {formType} и уже начали ее обработку.</p>
                    <p>Наш специалист свяжется с вами в ближайшее время (обычно в течение 24 часов).</p>
                    <br>
                    <p>С уважением,<br>Команда CREATIFY</p>
                    <hr>
                    <p style='font-size: 12px; color: #666;'>
                        Если у вас есть срочные вопросы, вы можете связаться с нами по телефону: +7 (903) 738-02-76
                    </p>
                </body>
                </html>";
                
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            
            using (var client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings["FromEmail"], "CREATIFY Digital Design"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                
                mailMessage.To.Add(toEmail);
                
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}