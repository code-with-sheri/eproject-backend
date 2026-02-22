/* 
 * The Client model represents a company or individual that has hired our security services.
 */

using System.ComponentModel.DataAnnotations;

namespace eproject_backend.Models
{
    public class Client
    {
        // Unique ID for each client
        public int Id { get; set; }
        
        // Name of the client company
        [Required]
        public string Name { get; set; } = string.Empty;
        
        // A short description of the services they are paying for
        [Required]
        public string ServicesAvailed { get; set; } = string.Empty;
        
        // Which employees are currently working at the client's site
        [Required]
        public string StaffAssigned { get; set; } = string.Empty;

        // Basic contact information
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
    }
}
