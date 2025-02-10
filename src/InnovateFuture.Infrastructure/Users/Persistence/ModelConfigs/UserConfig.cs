using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Users.Persistence.ModelConfigs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        
        // Changed default table name 
        builder.ToTable("Users");

        // Ignore unused fields
        builder.Ignore(u => u.LockoutEnabled);
        builder.Ignore(u => u.AccessFailedCount);
        builder.Ignore(u => u.PhoneNumberConfirmed);
        builder.Ignore(u => u.LockoutEnd);
        builder.Ignore(u => u.TwoFactorEnabled);
        builder.Ignore(u => u.ConcurrencyStamp);
        builder.Ignore(u => u.NormalizedEmail);
        builder.Ignore(u => u.NormalizedUserName);
        builder.Ignore(u => u.PhoneNumber);
        builder.Ignore(u => u.SecurityStamp);
        
        // Column Mappings
        builder.Property(u => u.Email).IsRequired();
        builder.Property(u => u.UserName).HasMaxLength(100).IsRequired();
        builder.Property(u=>u.PasswordHash).IsRequired();
        builder.Property(u => u.IdpSubject).HasColumnType("uuid").IsRequired(false);
        builder.Property(u => u.DefaultProfileId).HasColumnType("uuid").IsRequired(false);
        builder.Property(u => u.CreatedAt).HasColumnType("timestamptz").IsRequired();
        builder.Property(u => u.UpdatedAt).HasColumnType("timestamptz").IsRequired();

        // Set indexes
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_email");
        
        // navigation property
        builder.HasOne<Profile>()
            .WithOne()
            .HasForeignKey<User>(u => u.DefaultProfileId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
