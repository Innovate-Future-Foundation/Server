using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Profiles.Persistence.ModelConfigs;

public class ProfileConfig : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(p => p.ProfileId);
        
        // Set indexes
        builder.HasIndex(p=> new {p.UserId, p.RoleId, p.OrgId})
            .IsUnique()
            .HasDatabaseName("IX_Profiles_user_id_role_id_org_id");
        
        // Column Mappings
        builder.Property(p => p.ProfileId).HasColumnType("uuid").HasColumnName("profile_id").IsRequired();
        builder.Property(p => p.UserId).HasColumnType("uuid").HasColumnName("user_id").IsRequired();
        builder.Property(p => p.RoleId).HasColumnType("uuid").HasColumnName("role_id").IsRequired();
        builder.Property(p => p.OrgId).HasColumnType("uuid").HasColumnName("org_id"); // no need for platform admin
        builder.Property(p => p.Inviter).HasColumnType("uuid").HasColumnName("inviter");
        builder.Property(p => p.Supervisor).HasColumnType("uuid").HasColumnName("supervisor");
        builder.Property(p => p.Name).HasColumnName("name").HasMaxLength(100);

        builder.Property(p => p.Email).HasColumnName("email").HasMaxLength(100);
        builder.Property(p => p.Phone).HasColumnName("phone").HasMaxLength(50);
        builder.Property(p => p.Avatar).HasColumnName("avatar").HasMaxLength(500);
        builder.Property(p => p.IsActive).HasColumnType("boolean").HasColumnName("is_active").IsRequired();
        builder.Property(p => p.IsConfirmed).HasColumnType("boolean").HasColumnName("is_confirmed").IsRequired();
        builder.Property(p => p.CreatedAt).HasColumnType("timestamptz").HasColumnName("created_at").IsRequired();
        builder.Property(p => p.UpdatedAt).HasColumnType("timestamptz").HasColumnName("updated_at").IsRequired();
        
        // Relationships
        builder.HasOne(p => p.User)
            .WithMany(u => u.Profiles)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.Role)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(p => p.Organisation)
            .WithMany(o => o.Profiles)
            .HasForeignKey(p => p.OrgId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.InviterProfile)
            .WithMany()
            .HasForeignKey(p => p.Inviter)
            .OnDelete(DeleteBehavior.SetNull);

        

        builder.HasOne(p => p.SupervisorProfile)
            .WithMany()
            .HasForeignKey(p => p.Supervisor)
            .OnDelete(DeleteBehavior.SetNull);


    }
}
