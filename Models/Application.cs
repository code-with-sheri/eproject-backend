namespace eproject_backend.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int VacancyId { get; set; }
        public string Status { get; set; } = "Pending";
    }

}
