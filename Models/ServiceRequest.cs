using System.ComponentModel.DataAnnotations;

namespace eproject_backend.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string RequestedServiceName { get; set; } = string.Empty;

        public string? Status { get; set; } = "Pending";
        
        public DateTime RequestedAt { get; set; } = DateTime.Now;

        public int? AssignedEmployeeId { get; set; }
        public Employee? AssignedEmployee { get; set; }
    }
}
