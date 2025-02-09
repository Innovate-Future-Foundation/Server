using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Users.Persistence.ModelConfigs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Column Mappings
        builder.Property(u => u.IdpSubject).HasColumnType("uuid").HasColumnName("idp_subject").IsRequired(false);
        builder.Property(u => u.DefaultProfileId).HasColumnType("uuid").HasColumnName("profile_id").IsRequired(false);
        builder.Property(u => u.CreatedAt).HasColumnType("timestamptz").HasColumnName("created_at").IsRequired();
        builder.Property(u => u.UpdatedAt).HasColumnType("timestamptz").HasColumnName("updated_at").IsRequired();
        builder.Property(u => u.Email).IsRequired();
        
        // Set indexes
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_email");
    }
}
