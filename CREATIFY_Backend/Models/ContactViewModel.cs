// Models/ContactViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace CREATIFY_Backend.Models
{
    public class ContactViewModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Phone]
        public string Phone { get; set; }
        
        public string Service { get; set; }
        
        [Required]
        public string Message { get; set; }
        
        [Required]
        public bool Privacy { get; set; }
    }
}