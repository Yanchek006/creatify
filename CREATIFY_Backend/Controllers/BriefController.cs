// Controllers/BriefController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CREATIFY_Backend.Models;
using CREATIFY_Backend.Services;
using Microsoft.AspNetCore.Http;
using System;

namespace CREATIFY_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BriefController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly EmailService _emailService;
        
        public BriefController(DatabaseContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        
        [HttpPost]
        public async Task<IActionResult> SubmitBrief([FromBody] BriefViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errors = ModelState });
            }
            
            try
            {
                var submission = new FormSubmission
                {
                    FormType = "brief",
                    SubmittedAt = DateTime.UtcNow,
                    Name = model.ContactName,
                    Email = model.ContactEmail,
                    Phone = model.ContactPhone,
                    Company = model.Company,
                    Industry = model.Industry,
                    ProjectType = model.ProjectType,
                    ProjectDescription = model.Description,
                    Goals = model.Goals,
                    TargetAudience = model.TargetAudience,
                    Competitors = model.Competitors,
                    References = model.References,
                    ContentReady = model.Content != null ? string.Join(", ", model.Content) : "",
                    DesignStyle = model.Style,
                    ColorPalette = model.Colors,
                    Mood = model.Mood,
                    Budget = model.Budget,
                    Deadline = model.Deadline,
                    NdaRequired = model.Nda,
                    Newsletter = model.Newsletter,
                    Status = "new",
                    IpAddress = GetClientIp(),
                    UserAgent = Request.Headers["User-Agent"]
                };
                
                _context.FormSubmissions.Add(submission);
                await _context.SaveChangesAsync();
                
                // Send notification email to admin
                await _emailService.SendNotificationEmail(
                    "Новая заявка с сайта - Бриф",
                    GenerateBriefEmailBody(submission)
                );
                
                // Send auto-reply to user
                await _emailService.SendAutoReply(model.ContactEmail, model.ContactName, "заявку (бриф)");
                
                return Ok(new { 
                    success = true, 
                    message = "Бриф успешно отправлен! Мы свяжемся с вами в ближайшее время.",
                    id = submission.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Ошибка при отправке: " + ex.Message });
            }
        }
        
        private string GenerateBriefEmailBody(FormSubmission submission)
        {
            return $@"
                <h2>Новая заявка с сайта</h2>
                <p><strong>Тип:</strong> Бриф на услуги</p>
                <p><strong>Дата:</strong> {submission.SubmittedAt:yyyy-MM-dd HH:mm:ss}</p>
                <hr>
                <h3>Контактная информация:</h3>
                <p><strong>Имя:</strong> {submission.Name}</p>
                <p><strong>Email:</strong> {submission.Email}</p>
                <p><strong>Телефон:</strong> {submission.Phone}</p>
                <hr>
                <h3>Информация о проекте:</h3>
                <p><strong>Компания:</strong> {submission.Company}</p>
                <p><strong>Сфера:</strong> {submission.Industry}</p>
                <p><strong>Тип проекта:</strong> {submission.ProjectType}</p>
                <p><strong>Описание:</strong> {submission.ProjectDescription}</p>
                <p><strong>Цели:</strong> {submission.Goals}</p>
                <p><strong>Бюджет:</strong> {submission.Budget}</p>
                <p><strong>Сроки:</strong> {submission.Deadline}</p>
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