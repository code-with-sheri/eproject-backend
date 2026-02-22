/* 
 * The Vacancy model represents an open job position in the company.
 * These are displayed on the public website for candidates to apply for.
 */

namespace eproject_backend.Models
{
    public class Vacancy
    {
        // Unique ID for each job opening
        public int Id { get; set; }

        // Job title (e.g., "Supervisor", "Security Guard")
        public string Title { get; set; } = string.Empty;

        // Details about salary, shift hours, and job responsibilities.
        public string Description { get; set; } = string.Empty;
    }
}
