using System.ComponentModel.DataAnnotations;

namespace eproject_backend.Models
{
    public class Service
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
    }
}
