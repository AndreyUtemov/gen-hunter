using Microsoft.EntityFrameworkCore;
using GenHunter.Models;

namespace GenHunter.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Patient> AllPatients { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Явно указываем, что DbSet AllPatients соответствует таблице patients
        modelBuilder.Entity<Patient>().ToTable("patients");
        
        modelBuilder.Entity<Patient>()
            .Property(p => p.DOB)
            .HasColumnType("DATE")
            .HasConversion(
                d => d.ToDateTime(new TimeOnly(0, 0)), // DateOnly -> DateTime
                d => DateOnly.FromDateTime(d) // DateTime -> DateOnly
            );

        // Маппинг для enum Sex
        modelBuilder.Entity<Patient>()
            .Property(p => p.Sex)
            .HasConversion(
                v => v.ToString(), // Enum -> строка для хранения
                v => Enum.Parse<Sex>(v) // Строка -> enum при чтении
            );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")));

        }
    }
    
}