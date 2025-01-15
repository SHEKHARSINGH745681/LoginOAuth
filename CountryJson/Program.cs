using Microsoft.EntityFrameworkCore;
using CountryJson.DataSeed;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext for database connection using Npgsql (PostgreSQL)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger for API documentation (optional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed data during application startup if needed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var seeder = new Seed(context);

    try
    {
        // Call the seeding method
        await seeder.SeedDataAsync();
    }
    catch (Exception ex)
    {
        // Log the exception (use logging libraries like Serilog or NLog if needed)
        Console.WriteLine($"An error occurred during seeding: {ex.Message}");
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
