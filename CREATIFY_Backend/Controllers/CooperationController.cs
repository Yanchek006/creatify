// Controllers/CooperationController.cs
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CREATIFY_Backend.Models;
using CREATIFY_Backend.Services;
using System;

namespace CREATIFY_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CooperationController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly EmailService _emailService;
        
        public CooperationController(DatabaseContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        
        [HttpPost]
        public async Task<IActionResult> SubmitCooperation([FromBody] CooperationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errors = ModelState });
            }
            
            try
            {
                var submission = new FormSubmission
                {
                    FormType = "cooperation",
                    SubmittedAt = DateTime.UtcNow,
                    Name = model.ContactName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Company = model.Company,
                    CooperationType = model.CooperationType,
                    Portfolio = model.Portfolio,
                    Experience = model.Experience,
                    Message = model.Message,
                    Status = "new",
                    IpAddress = GetClientIp(),
                    UserAgent = Request.Headers["User-Agent"]
                };
                
                _context.FormSubmissions.Add(submission);
                await _context.SaveChangesAsync();
                
                // Send notification email to admin
                await _emailService.SendNotificationEmail(
                    "Новая заявка на сотрудничество",
                    GenerateCooperationEmailBody(submission)
                );
                
                // Send auto-reply to user
                await _emailService.SendAutoReply(model.Email, model.ContactName, "заявку на сотрудничество");
                
                return Ok(new { 
                    success = true, 
                    message = "Заявка на сотрудничество успешно отправлена! Мы свяжемся с вами в ближайшее время.",
                    id = submission.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Ошибка при отправке: " + ex.Message });
            }
        }
        
        private string GenerateCooperationEmailBody(FormSubmission submission)
        {
            return $@"
                <h2>Новая заявка на сотрудничество</h2>
                <p><strong>Дата:</strong> {submission.SubmittedAt:yyyy-MM-dd HH:mm:ss}</p>
                <hr>
                <h3>Информация о партнере:</h3>
                <p><strong>Компания:</strong> {submission.Company}</p>
                <p><strong>Контактное лицо:</strong> {submission.Name}</p>
                <p><strong>Email:</strong> {submission.Email}</p>
                <p><strong>Телефон:</strong> {submission.Phone}</p>
                <p><strong>Тип сотрудничества:</strong> {submission.CooperationType}</p>
                <hr>
                <h3>Портфолио:</h3>
                <p>{submission.Portfolio}</p>
                <h3>Опыт:</h3>
                <p>{submission.Experience}</p>
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