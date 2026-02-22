/* 
 * The Application model represents a job application submitted by a candidate.
 * It stores personal information and a link to the candidate's CV file.
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace eproject_backend.Models
{
    public class Application
    {
        // Unique ID for each application
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // CNIC number for identification
        [Required]
        public string Cnic { get; set; } = string.Empty;

        public int Age { get; set; }

        [Required]
        public string Phone { get; set; } = string.Empty;
        
        // Which job vacancy is the candidate applying for?
        [Required]
        public int VacancyId { get; set; }
        
        // Starts as "Pending" and can be "Approved" or "Rejected" by an admin.
        public string? Status { get; set; } = "Pending";

        // Stores the filename of the uploaded CV in the database.
        public string? CvPath { get; set; }

        // [NotMapped] tells Entity Framework NOT to create a column for this in SQL.
        // It's used only for receiving the actual file during an upload.
        [NotMapped]
        public IFormFile? Cv { get; set; }
    }
}
