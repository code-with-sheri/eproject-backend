/* 
 * This is the 'main' file of the backend application.
 * It sets up the server, connects to the database, and defines how the API behaves.
 */

using eproject_backend.Data;
using eproject_backend.Models;
using eproject_backend.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Controllers: This tells the server to look for Controller classes (which handle API requests).
builder.Services.AddControllers();

// 2. Add Swagger: This creates a useful web page for testing our API endpoints.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Add DbContext: This connects our app to the SQL Server database using the connection string in 'appsettings.json'.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// 4. Add Repositories: These are helper classes that handle data operations. 
// 'AddScoped' means a new instance is created for every web request.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

// 5. CORS (Cross-Origin Resource Sharing): 
// This allows our Angular frontend (running on a different port) to talk to this backend.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

// 6. Middleware: These are steps that every request goes through.
if (app.Environment.IsDevelopment())
{
    // If we are developing, show the Swagger UI for easy testing.
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS policy we defined above
app.UseCors("AllowAngular");

// Enable the server to serve static files like images (stored in wwwroot)
app.UseStaticFiles();

// Basic authorization setup
app.UseAuthorization();

// Tell the server to map the URLs to our Controller functions
app.MapControllers();

// 7. Database Seeding: This block runs when the app starts.
// It checks if the database has any data. If not, it adds some initial data so the app isn't empty.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Automatically creates the database tables if they don't exist (Migrations)
    db.Database.Migrate();

    // Check if there is an Admin user. If not, create one.
    if (!db.Users.AsNoTracking().Any(u => u.Role == "Admin"))
    {
        db.Users.Add(new User
        {
            Name = "Admin",
            Email = "admin@system.com",
            Password = "admin123", // In a real app, passwords should be encrypted!
            Role = "Admin"
        });
        db.SaveChanges(); // Saves the new admin to the database
    }

    // Add some starting Vacancies (Job openings) if none exist
    if (!db.Vacancies.AsNoTracking().Any())
    {
        db.Vacancies.AddRange(
            new Vacancy { Title = "Security Guard", Description = "Salary: 30,000 PKR / month. 8 Hours Shift." },
            new Vacancy { Title = "Supervisor", Description = "Salary: 45,000 PKR / month. Day Shift." },
            new Vacancy { Title = "Control Room Operator", Description = "Salary: 40,000 PKR / month. Night Shift." }
        );
        db.SaveChanges();
    }

    // Add some starting Services (What the company offers) if none exist
    if (!db.Services.AsNoTracking().Any())
    {
        db.Services.AddRange(
            new Service { Name = "Office Security", Description = "Professional guarding for corporate offices." },
            new Service { Name = "Home Security", Description = "24/7 protection for residential areas." },
            new Service { Name = "Event Security", Description = "Specialized security for public and private events." }
        );
        db.SaveChanges();
    }
}

// 8. Run the App: Start listening for requests!
app.Run();
