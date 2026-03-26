// Controllers/ExportController.cs
using Microsoft.AspNetCore.Mvc;
using CREATIFY_Backend.Services;
using Microsoft.AspNetCore.Authorization;

namespace CREATIFY_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protect this endpoint with authentication
    public class ExportController : ControllerBase
    {
        private readonly ExcelExportService _excelExportService;
        
        public ExportController(ExcelExportService excelExportService)
        {
            _excelExportService = excelExportService;
        }
        
        [HttpGet("all")]
        public IActionResult ExportAllToExcel()
        {
            var fileBytes = _excelExportService.ExportAllSubmissionsToExcel();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"all_submissions_{DateTime.Now:yyyy-MM-dd}.xlsx");
        }
        
        [HttpGet("brief")]
        public IActionResult ExportBriefToExcel()
        {
            var fileBytes = _excelExportService.ExportByFormType("brief");
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"brief_submissions_{DateTime.Now:yyyy-MM-dd}.xlsx");
        }
        
        [HttpGet("contact")]
        public IActionResult ExportContactToExcel()
        {
            var fileBytes = _excelExportService.ExportByFormType("contact");
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"contact_submissions_{DateTime.Now:yyyy-MM-dd}.xlsx");
        }
        
        [HttpGet("cooperation")]
        public IActionResult ExportCooperationToExcel()
        {
            var fileBytes = _excelExportService.ExportByFormType("cooperation");
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"cooperation_submissions_{DateTime.Now:yyyy-MM-dd}.xlsx");
        }
    }
}