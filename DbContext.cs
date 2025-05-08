using ef_softdeletion_audit.Entities;
using Microsoft.EntityFrameworkCore;

namespace ef_softdeletion_audit;

class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            e.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(60);
            e.HasIndex(u => u.Email, "IX_User_Email")
                .IsUnique();

            e.HasQueryFilter(u => !u.IsDeleted); // Apply global filter for soft deletion
            e.HasIndex(u => u.IsDeleted)
                .HasFilter("IsDelete = 0");
        });
    }

}
