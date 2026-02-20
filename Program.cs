using eproject_backend.Data;
using eproject_backend.Models;
using eproject_backend.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

// CORS
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

// ✅ Seed Admin (runs once)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Auto-apply migrations
    db.Database.Migrate();

    if (!db.Users.AsNoTracking().Any(u => u.Role == "Admin"))
    {
        db.Users.Add(new User
        {
            Name = "Admin",
            Email = "admin@system.com",
            Password = "admin123",
            Role = "Admin"
        });
        db.SaveChanges();
    }

    if (!db.Vacancies.AsNoTracking().Any())
    {
        db.Vacancies.AddRange(
            new Vacancy { Title = "Security Guard", Description = "Salary: 30,000 PKR / month. 8 Hours Shift." },
            new Vacancy { Title = "Supervisor", Description = "Salary: 45,000 PKR / month. Day Shift." },
            new Vacancy { Title = "Control Room Operator", Description = "Salary: 40,000 PKR / month. Night Shift." }
        );
        db.SaveChanges();
    }

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

app.Run();
