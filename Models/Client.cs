using System.ComponentModel.DataAnnotations;

namespace eproject_backend.Models
{
    public class Client
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string ServicesAvailed { get; set; }
        
        [Required]
        public string StaffAssigned { get; set; }

        public string? ContactEmail { get; set; }

        public string? ContactPhone { get; set; }
    }
}
