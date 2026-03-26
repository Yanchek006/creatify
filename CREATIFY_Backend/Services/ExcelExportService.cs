// Services/ExcelExportService.cs
using OfficeOpenXml;
using System.Data;
using System.IO;
using System.Linq;
using CREATIFY_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace CREATIFY_Backend.Services
{
    public class ExcelExportService
    {
        private readonly DatabaseContext _context;
        
        public ExcelExportService(DatabaseContext context)
        {
            _context = context;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        
        public byte[] ExportAllSubmissionsToExcel()
        {
            var submissions = _context.FormSubmissions
                .OrderByDescending(s => s.SubmittedAt)
                .ToList();
                
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Form Submissions");
                
                // Headers
                string[] headers = {
                    "ID", "Form Type", "Submitted At", "Status", "Name", "Email", "Phone",
                    "Company", "Industry", "Project Type", "Project Description", "Goals",
                    "Target Audience", "Competitors", "References", "Content Ready",
                    "Design Style", "Color Palette", "Mood", "Budget", "Deadline",
                    "NDA Required", "Newsletter", "Service", "Message", "Cooperation Type",
                    "Portfolio", "Experience", "IP Address", "Notes"
                };
                
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }
                
                // Data rows
                for (int i = 0; i < submissions.Count; i++)
                {
                    var s = submissions[i];
                    int row = i + 2;
                    
                    worksheet.Cells[row, 1].Value = s.Id;
                    worksheet.Cells[row, 2].Value = s.FormType;
                    worksheet.Cells[row, 3].Value = s.SubmittedAt.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cells[row, 4].Value = s.Status;
                    worksheet.Cells[row, 5].Value = s.Name;
                    worksheet.Cells[row, 6].Value = s.Email;
                    worksheet.Cells[row, 7].Value = s.Phone;
                    worksheet.Cells[row, 8].Value = s.Company;
                    worksheet.Cells[row, 9].Value = s.Industry;
                    worksheet.Cells[row, 10].Value = s.ProjectType;
                    worksheet.Cells[row, 11].Value = s.ProjectDescription;
                    worksheet.Cells[row, 12].Value = s.Goals;
                    worksheet.Cells[row, 13].Value = s.TargetAudience;
                    worksheet.Cells[row, 14].Value = s.Competitors;
                    worksheet.Cells[row, 15].Value = s.References;
                    worksheet.Cells[row, 16].Value = s.ContentReady;
                    worksheet.Cells[row, 17].Value = s.DesignStyle;
                    worksheet.Cells[row, 18].Value = s.ColorPalette;
                    worksheet.Cells[row, 19].Value = s.Mood;
                    worksheet.Cells[row, 20].Value = s.Budget;
                    worksheet.Cells[row, 21].Value = s.Deadline;
                    worksheet.Cells[row, 22].Value = s.NdaRequired ? "Yes" : "No";
                    worksheet.Cells[row, 23].Value = s.Newsletter ? "Yes" : "No";
                    worksheet.Cells[row, 24].Value = s.Service;
                    worksheet.Cells[row, 25].Value = s.Message;
                    worksheet.Cells[row, 26].Value = s.CooperationType;
                    worksheet.Cells[row, 27].Value = s.Portfolio;
                    worksheet.Cells[row, 28].Value = s.Experience;
                    worksheet.Cells[row, 29].Value = s.IpAddress;
                    worksheet.Cells[row, 30].Value = s.Notes;
                }
                
                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();
                
                return package.GetAsByteArray();
            }
        }
        
        public byte[] ExportByFormType(string formType)
        {
            var submissions = _context.FormSubmissions
                .Where(s => s.FormType == formType)
                .OrderByDescending(s => s.SubmittedAt)
                .ToList();
                
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add($"{formType}_Submissions");
                
                // Similar headers but simplified based on form type
                var headers = GetHeadersByFormType(formType);
                
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }
                
                // Populate data
                for (int i = 0; i < submissions.Count; i++)
                {
                    var s = submissions[i];
                    int row = i + 2;
                    PopulateRowByFormType(worksheet, row, s, formType);
                }
                
                worksheet.Cells.AutoFitColumns();
                return package.GetAsByteArray();
            }
        }
        
        private string[] GetHeadersByFormType(string formType)
        {
            if (formType == "brief")
            {
                return new[] { "ID", "Date", "Company", "Industry", "Project Type", "Description", 
                              "Goals", "Budget", "Deadline", "Name", "Email", "Phone", "Status" };
            }
            else if (formType == "contact")
            {
                return new[] { "ID", "Date", "Name", "Email", "Phone", "Service", "Message", "Status" };
            }
            else
            {
                return new[] { "ID", "Date", "Company", "Name", "Email", "Phone", "Cooperation Type", 
                              "Experience", "Message", "Status" };
            }
        }
        
        private void PopulateRowByFormType(ExcelWorksheet worksheet, int row, FormSubmission s, string formType)
        {
            if (formType == "brief")
            {
                worksheet.Cells[row, 1].Value = s.Id;
                worksheet.Cells[row, 2].Value = s.SubmittedAt.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 3].Value = s.Company;
                worksheet.Cells[row, 4].Value = s.Industry;
                worksheet.Cells[row, 5].Value = s.ProjectType;
                worksheet.Cells[row, 6].Value = s.ProjectDescription;
                worksheet.Cells[row, 7].Value = s.Goals;
                worksheet.Cells[row, 8].Value = s.Budget;
                worksheet.Cells[row, 9].Value = s.Deadline;
                worksheet.Cells[row, 10].Value = s.Name;
                worksheet.Cells[row, 11].Value = s.Email;
                worksheet.Cells[row, 12].Value = s.Phone;
                worksheet.Cells[row, 13].Value = s.Status;
            }
            else if (formType == "contact")
            {
                worksheet.Cells[row, 1].Value = s.Id;
                worksheet.Cells[row, 2].Value = s.SubmittedAt.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 3].Value = s.Name;
                worksheet.Cells[row, 4].Value = s.Email;
                worksheet.Cells[row, 5].Value = s.Phone;
                worksheet.Cells[row, 6].Value = s.Service;
                worksheet.Cells[row, 7].Value = s.Message;
                worksheet.Cells[row, 8].Value = s.Status;
            }
            else
            {
                worksheet.Cells[row, 1].Value = s.Id;
                worksheet.Cells[row, 2].Value = s.SubmittedAt.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 3].Value = s.Company;
                worksheet.Cells[row, 4].Value = s.Name;
                worksheet.Cells[row, 5].Value = s.Email;
                worksheet.Cells[row, 6].Value = s.Phone;
                worksheet.Cells[row, 7].Value = s.CooperationType;
                worksheet.Cells[row, 8].Value = s.Experience;
                worksheet.Cells[row, 9].Value = s.Message;
                worksheet.Cells[row, 10].Value = s.Status;
            }
        }
    }
}