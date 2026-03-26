// Models/BriefViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace CREATIFY_Backend.Models
{
    public class BriefViewModel
    {
        [Required]
        public string Company { get; set; }
        
        [Required]
        public string Industry { get; set; }
        
        [Required]
        public string ProjectType { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public string Goals { get; set; }
        
        public string TargetAudience { get; set; }
        public string Competitors { get; set; }
        public string References { get; set; }
        public string[] Content { get; set; }
        public string Style { get; set; }
        public string Colors { get; set; }
        public string Mood { get; set; }
        
        [Required]
        public string ContactName { get; set; }
        
        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }
        
        [Phone]
        public string ContactPhone { get; set; }
        
        public string CompanySite { get; set; }
        
        [Required]
        public string Budget { get; set; }
        
        public string Deadline { get; set; }
        public bool Nda { get; set; }
        public bool Newsletter { get; set; }
        
        [Required]
        public bool PrivacyAgreement { get; set; }
        
        [Required]
        public bool OfferAgreement { get; set; }
    }
}