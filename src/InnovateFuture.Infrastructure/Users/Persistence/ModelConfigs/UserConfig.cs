

using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Users.Persistence.ModelConfigs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(p => p.UserId);
        
        // Set indexes
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_email");
        
        builder.HasIndex(u => u.CognitoUuid)
            .IsUnique()
            .HasDatabaseName("IX_Users_cognito_uuid");
        
        // Column Mappings
        builder.Property(u => u.UserId).HasColumnType("uuid").HasColumnName("user_id").IsRequired();
        // todo: cognito_uuid -> char(x)
        builder.Property(u => u.CognitoUuid).HasColumnName("cognito_uuid").HasMaxLength(500).IsRequired();
        builder.Property(u => u.DefaultProfile).HasColumnType("uuid").HasColumnName("default_profile");
        builder.Property(u => u.GivenName).HasColumnName("given_name").HasMaxLength(100);
        builder.Property(u => u.FamilyName).HasColumnName("family_name").HasMaxLength(100);
        builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
        builder.Property(u => u.Phone).HasColumnName("phone").HasMaxLength(50);
        builder.Property(u => u.Birthday).HasColumnType("date").HasColumnName("birthday");
        builder.Property(u => u.CreatedAt).HasColumnType("timestamptz").HasColumnName("created_at").IsRequired();
        builder.Property(u => u.UpdatedAt).HasColumnType("timestamptz").HasColumnName("updated_at").IsRequired();
        
        // navigation property
        builder.HasMany(u => u.Profiles)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
