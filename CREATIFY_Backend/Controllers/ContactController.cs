// Controllers/ContactController.cs
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CREATIFY_Backend.Models;
using CREATIFY_Backend.Services;
using System;

namespace CREATIFY_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly EmailService _emailService;
        
        public ContactController(DatabaseContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        
        [HttpPost]
        public async Task<IActionResult> SubmitContact([FromBody] ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errors = ModelState });
            }
            
            try
            {
                var submission = new FormSubmission
                {
                    FormType = "contact",
                    SubmittedAt = DateTime.UtcNow,
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    Service = model.Service,
                    Message = model.Message,
                    Status = "new",
                    IpAddress = GetClientIp(),
                    UserAgent = Request.Headers["User-Agent"]
                };
                
                _context.FormSubmissions.Add(submission);
                await _context.SaveChangesAsync();
                
                // Send notification email to admin
                await _emailService.SendNotificationEmail(
                    "Новое сообщение с сайта - Контакты",
                    GenerateContactEmailBody(submission)
                );
                
                // Send auto-reply to user
                await _emailService.SendAutoReply(model.Email, model.Name, "сообщение");
                
                return Ok(new { 
                    success = true, 
                    message = "Сообщение успешно отправлено! Мы свяжемся с вами в ближайшее время.",
                    id = submission.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Ошибка при отправке: " + ex.Message });
            }
        }
        
        private string GenerateContactEmailBody(FormSubmission submission)
        {
            return $@"
                <h2>Новое сообщение с сайта</h2>
                <p><strong>Тип:</strong> Контактная форма</p>
                <p><strong>Дата:</strong> {submission.SubmittedAt:yyyy-MM-dd HH:mm:ss}</p>
                <hr>
                <h3>Контактная информация:</h3>
                <p><strong>Имя:</strong> {submission.Name}</p>
                <p><strong>Email:</strong> {submission.Email}</p>
                <p><strong>Телефон:</strong> {submission.Phone}</p>
                <p><strong>Услуга:</strong> {submission.Service}</p>
                <hr>
                <h3>Сообщение:</h3>
                <p>{submission.Message}</p>
                <hr>
                <p><strong>IP адрес:</strong> {submission.IpAddress}</p>
            ";
        }
        
        private string GetClientIp()
        {
            var ip = Request.Headers["X-Forwarded-For"].ToString();
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            }
            return ip;
        }
    }
}