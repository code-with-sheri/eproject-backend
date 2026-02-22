/* 
 * AppDbContext is the 'bridge' between our C# code and the SQL database.
 * It manages the database tables (DbSets) and handles the actual saving/fetching of data.
 */

using Microsoft.EntityFrameworkCore;
using eproject_backend.Models;

namespace eproject_backend.Data
{
    public class AppDbContext : DbContext
    {
        // This constructor passes the database configuration (from Program.cs) to the base DbContext.
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Each DbSet represents a table in the SQL Server database.
        // If we add a DbSet here, Entity Framework will create a table for it.

        public DbSet<User> Users { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This is where we can add special rules for our tables if needed.
            base.OnModelCreating(modelBuilder);
        }
    }
}
