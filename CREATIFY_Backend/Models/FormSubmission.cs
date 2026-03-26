// Models/FormSubmission.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CREATIFY_Backend.Models
{
    public class FormSubmission
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string FormType { get; set; } // "brief", "contact", "cooperation"
        
        [Required]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        
        // Common fields
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        
        // Brief specific fields
        public string Company { get; set; }
        public string Industry { get; set; }
        public string ProjectType { get; set; }
        public string ProjectDescription { get; set; }
        public string Goals { get; set; }
        public string TargetAudience { get; set; }
        public string Competitors { get; set; }
        public string References { get; set; }
        public string ContentReady { get; set; }
        public string DesignStyle { get; set; }
        public string ColorPalette { get; set; }
        public string Mood { get; set; }
        public string Budget { get; set; }
        public string Deadline { get; set; }
        public bool NdaRequired { get; set; }
        public bool Newsletter { get; set; }
        
        // Contact form fields
        public string Service { get; set; }
        public string Message { get; set; }
        
        // Cooperation form fields
        public string CooperationType { get; set; }
        public string Portfolio { get; set; }
        public string Experience { get; set; }
        
        // Status tracking
        public string Status { get; set; } = "new"; // new, in_progress, completed, rejected
        public string Notes { get; set; }
        
        // IP and User Agent for security
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}