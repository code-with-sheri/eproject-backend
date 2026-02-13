using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

public class Application
{
    public int Id { get; set; }
    public string name { get; set; }      // 👈 lowercase
    public string email { get; set; }     // 👈 lowercase
    public int vacancyId { get; set; }    // 👈 lowercase
    public string Status { get; set; }

    public string CvPath { get; set; }

    [NotMapped]
    public IFormFile cv { get; set; }     // 👈 lowercase
}