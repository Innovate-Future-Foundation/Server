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
    public virtual DbSet<Activity> Activities { get; set; }
    public virtual DbSet<Day> Days { get; set; }
    public virtual DbSet<Tour> Tours { get; set; }
    public virtual DbSet<StudentTourEnrollment> StudentTourEnrollments { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresEnum<RoleEnum>(name: "role_enum");
        modelBuilder.HasPostgresEnum<SubscriptionEnum>(name: "subscription_enum");
        modelBuilder.HasPostgresEnum<OrgStatusEnum>(name: "org_status_enum");
        modelBuilder.HasPostgresEnum<TourStatusEnum>(name: "tour_status_enum");
        modelBuilder.HasPostgresEnum<EnrollmentStatusEnum>(name: "enrollment_status_enum");

        // Remove unnecessary identity tables
        modelBuilder.Ignore<IdentityRole<Guid>>();
        modelBuilder.Ignore<IdentityUserRole<Guid>>();

        // Apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
