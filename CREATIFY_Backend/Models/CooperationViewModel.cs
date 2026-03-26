// Models/CooperationViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace CREATIFY_Backend.Models
{
    public class CooperationViewModel
    {
        [Required]
        public string Company { get; set; }
        
        [Required]
        public string ContactName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Phone]
        public string Phone { get; set; }
        
        [Required]
        public string CooperationType { get; set; }
        
        public string Website { get; set; }
        public string Portfolio { get; set; }
        public string Experience { get; set; }
        
        [Required]
        public string Message { get; set; }
        
        [Required]
        public bool Privacy { get; set; }
    }
}