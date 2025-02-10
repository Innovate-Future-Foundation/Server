using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Profiles.Persistence.ModelConfigs;

public class ProfileConfig : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(p => p.Id);
        
        // Column Mappings
        builder.Property(p => p.Id).HasColumnType("uuid").IsRequired();
        builder.Property(p => p.UserId).HasColumnType("uuid").IsRequired();
        builder.Property(p => p.Role).HasColumnType("role_enum").IsRequired();
        builder.Property(p => p.OrgId).HasColumnType("uuid").IsRequired(false);
        builder.Property(p => p.Inviter).HasColumnType("uuid").IsRequired(false);
        builder.Property(p => p.Supervisor).HasColumnType("uuid").IsRequired(false);
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired(false);
        builder.Property(p => p.Email).HasMaxLength(100).IsRequired(false);
        builder.Property(p => p.Phone).HasMaxLength(50).IsRequired(false);
        builder.Property(p => p.AvatarUrl).HasMaxLength(500).IsRequired(false);
        builder.Property(p => p.IsActive).HasColumnType("boolean").IsRequired();
        builder.Property(p => p.IsConfirmed).HasColumnType("boolean").IsRequired();
        builder.Property(p => p.CreatedAt).HasColumnType("timestamptz").IsRequired();
        builder.Property(p => p.UpdatedAt).HasColumnType("timestamptz").IsRequired();
        
        // Set indexes
        builder.HasIndex(p=> new {p.UserId, p.Role, p.OrgId})
            .IsUnique()
            .HasDatabaseName("IX_Profiles_user_id_role_org_id");
        
        // Relationships
        builder.HasOne(p => p.User)
            .WithMany(u => u.Profiles)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.HasOne(p => p.Organisation)
            .WithMany(o => o.Profiles)
            .HasForeignKey(p => p.OrgId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        builder.HasOne(p => p.InviterProfile)
            .WithMany()
            .HasForeignKey(p => p.Inviter)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
        
        builder.HasOne(p => p.SupervisorProfile)
            .WithMany()
            .HasForeignKey(p => p.Supervisor)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
