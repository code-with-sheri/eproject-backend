using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace eproject_backend.Models
{
    public class Application
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Cnic { get; set; } = string.Empty;

        public int Age { get; set; }

        [Required]
        public string Phone { get; set; } = string.Empty;
        
        [Required]
        public int VacancyId { get; set; }
        
        public string? Status { get; set; } = "Pending";

        public string? CvPath { get; set; }

        [NotMapped]
        public IFormFile? Cv { get; set; }
    }
}
