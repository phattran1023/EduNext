using EduNext.Models;
using Microsoft.EntityFrameworkCore;

namespace EduNext;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<ImageUrls> ImageUrls { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define relationships using Fluent API

        // One-to-Many relationship between Department and Student
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Department)
            .WithMany() // Remove the reference to Students here
            .HasForeignKey(s => s.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Many-to-Many relationship between Student and Course through Enrollment
        modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.CourseId });

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
        // One-to-Many relationship between Student and ImageUrls
        modelBuilder.Entity<Student>()
              .HasMany(s => s.ImageUrls)
              .WithOne(iu => iu.Student)
              .HasForeignKey(iu => iu.StudentId);
    }

}
