/* 
 * The Employee model represents a staff member (like a security guard).
 * This class includes more personal details compared to the basic User model.
 */

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace eproject_backend.Models
{
    public class Employee
    {
        // Unique ID for each employee (Primary Key)
        public int Id { get; set; }
        
        // [Required] tells the database that this field must be filled and cannot be empty.
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress] // Ensures the email is in a valid format (e.g., test@test.com)
        public string Email { get; set; } = string.Empty;

        // Optional address field
        public string? Address { get; set; }
        
        [Required]
        public string ContactNumber { get; set; } = string.Empty;
        
        public string? EducationalQualification { get; set; }
        
        // A special code like "EMP-ABC-123" to identify employees in the system.
        [Required]
        public string EmployeeCode { get; set; } = string.Empty;
        
        // The department they work in (e.g., "Operations", "Sales")
        public string? Department { get; set; }
        
        // Their role (e.g., "Supervisor", "Guard")
        public string? Role { get; set; }
        
        // Their rank or grade in the company
        public string? Grade { get; set; }
        
        // The client site they are currently assigned to protect
        public string? Client { get; set; }
        
        // Any special awards or achievements
        public string? Achievements { get; set; }
        
        // This is the password they use to log in. 
        // Again, [JsonIgnore] is used for security so it's not sent out of the backend.
        [Required]
        [JsonIgnore]
        public string Password { get; set; } = "123456"; // Default password for new employees
    }
}
