using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data;

public class DataContextEF : DbContext
{
    private readonly IConfiguration _config;
    private readonly string? _connectionString;

    public DataContextEF(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("DefaultConnection");
    }

    // Needed to map the data models to the database
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserSalary> UserSalary { get; set; }
    public virtual DbSet<UserJobInfo> UserJobInfo { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Needs Microsoft.EntityframeworkCore.SqlServer which allows to use SQL Server with EF
        if (!optionsBuilder.IsConfigured) // Needed to avoid error when running migrations
        {
            optionsBuilder.UseSqlServer(
                _connectionString,
                optionsBuilder => optionsBuilder.EnableRetryOnFailure() // Needed to retry connection if it fails
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Needs Microsoft.EntityframeworkCore.Relational which allows to use relational databases with EF
        modelBuilder.HasDefaultSchema("TutorialAppSchema");
        modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.UserId); // Primary key is UserId
        modelBuilder.Entity<UserSalary>().HasKey(us => us.UserId);
        modelBuilder.Entity<UserJobInfo>().HasKey(uji => uji.UserId);
    }
}
