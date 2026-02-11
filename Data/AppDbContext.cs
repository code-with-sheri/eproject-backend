using Microsoft.EntityFrameworkCore;
using eproject_backend.Models;

namespace eproject_backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Application> Applications { get; set; }
    }
}
