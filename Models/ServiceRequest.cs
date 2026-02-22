/* 
 * The ServiceRequest model represents a request from a client for security services.
 * It's essentially a lead or a task that needs to be handled by the company.
 */

using System.ComponentModel.DataAnnotations;

namespace eproject_backend.Models
{
    public class ServiceRequest
    {
        // Unique ID for each request
        public int Id { get; set; }
        
        // Name of the person or company requesting the service
        [Required]
        public string Name { get; set; } = string.Empty;
        
        // Contact details for the person who made the request
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        // Which service they are interested in (e.g., "Office Security")
        [Required]
        public string RequestedServiceName { get; set; } = string.Empty;

        // The status starts as "Pending" and changes to "Assigned" when an admin picks someone for it.
        public string? Status { get; set; } = "Pending";
        
        // The date and time when the request was submitted.
        public DateTime RequestedAt { get; set; } = DateTime.Now;

        // If an employee is assigned to handle this request, their ID will be stored here.
        public int? AssignedEmployeeId { get; set; }
        
        // This is a 'Navigation Property'. 
        // It allows us to easily access the full employee object from this request.
        public Employee? AssignedEmployee { get; set; }
    }
}
