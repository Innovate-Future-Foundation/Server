using InnovateFuture.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using InnovateFuture.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace InnovateFuture.Infrastructure.Common.Persistence;
public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public virtual DbSet<Organisation> Organisations { get; set; }
    public virtual DbSet<Profile> Profiles { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresEnum<RoleEnum>(name: "role_enum");
        // Apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        // Changed default table name 
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("Users");
        });
        
        // Remove unnecessary identity tables
        modelBuilder.Ignore<IdentityRole<Guid>>();
        modelBuilder.Ignore<IdentityUserRole<Guid>>();
    }
}
