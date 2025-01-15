using CountryJson.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<State> States { get; set; }
    public DbSet<District> Districts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Define the relationship between State and District
        modelBuilder.Entity<State>()
            .HasMany(s => s.Districts)
            .WithOne() // No navigation property on the District class
            .HasForeignKey("StateId"); // Assuming you have a StateId in District
    }
}
