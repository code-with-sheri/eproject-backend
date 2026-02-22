/* 
 * The Service model represents a security product or service offered by the company.
 * Examples: "Armed Guards", "CCTV Monitoring", "Event Security".
 */

using System.ComponentModel.DataAnnotations;

namespace eproject_backend.Models
{
    public class Service
    {
        // Unique ID for each service
        public int Id { get; set; }
        
        // Name of the service (e.g., "Personal Bodyguard")
        [Required]
        public string Name { get; set; } = string.Empty;
        
        // Detailed description of what the service includes
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
