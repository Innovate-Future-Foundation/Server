

using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Roles.Persistence.ModelConfigs;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(p => p.RoleId);
        // Column Mappings
        builder.Property(r => r.RoleId).HasColumnType("uuid").HasColumnName("role_id").IsRequired();
        builder.Property(r => r.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.Property(r => r.CodeName).HasColumnType("smallint").HasColumnName("code_name").IsRequired();
        builder.Property(r => r.Description).HasColumnName("description").HasMaxLength(500);
        builder.Property(r => r.CreatedAt).HasColumnType("timestamptz").HasColumnName("created_at").IsRequired();
        builder.Property(r => r.UpdatedAt).HasColumnType("timestamptz").HasColumnName("updated_at").IsRequired();
    }
}
