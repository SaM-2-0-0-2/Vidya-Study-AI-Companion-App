using Microsoft.EntityFrameworkCore;
namespace Vidya_AI_Study_Companion.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Summary> Summaries { get; set; }
    public DbSet<StudyMaterial> StudyMaterials { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Summaries)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.StudyMaterials)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Feedbacks)
            .WithOne(f => f.User)
            .HasForeignKey(f => f.UserId);

        modelBuilder.Entity<Tag>()
            .HasMany(t => t.StudyMaterials)
            .WithOne(sm => sm.Tag)
            .HasForeignKey(sm => sm.TagId);

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>(); // Store enum as string

    }
}
