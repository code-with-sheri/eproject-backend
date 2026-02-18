using System.ComponentModel.DataAnnotations;

namespace eproject_backend.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string RequestedServiceName { get; set; }

        public string? Status { get; set; } = "Pending";
        
        public DateTime RequestedAt { get; set; } = DateTime.Now;
    }
}
