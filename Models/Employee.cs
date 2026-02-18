using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace eproject_backend.Models
{
    public class Employee
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? Address { get; set; }
        
        [Required]
        public string ContactNumber { get; set; }
        
        public string? EducationalQualification { get; set; }
        
        [Required]
        public string EmployeeCode { get; set; }
        
        public string? Department { get; set; }
        
        public string? Role { get; set; }
        
        public string? Grade { get; set; }
        
        public string? Client { get; set; }
        
        public string? Achievements { get; set; }
        
        [Required]
        [JsonIgnore]
        public string Password { get; set; } = "123456"; // Default password
    }
}
