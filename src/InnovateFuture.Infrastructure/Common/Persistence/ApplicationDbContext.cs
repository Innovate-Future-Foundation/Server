using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnovateFuture.Infrastructure.Common.Persistence;
public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Organisation> Organisations { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        var roles = DataSeed.GetRoles();
        modelBuilder.Entity<Role>().HasData(roles);
        
        var organisations = DataSeed.GetOrganisations();
        modelBuilder.Entity<Organisation>().HasData(organisations);

        var users = DataSeed.GetUsers();
        modelBuilder.Entity<User>().HasData(users);

        var profiles = DataSeed.GetProfiles();
        modelBuilder.Entity<Profile>().HasData(profiles);
    }
}
