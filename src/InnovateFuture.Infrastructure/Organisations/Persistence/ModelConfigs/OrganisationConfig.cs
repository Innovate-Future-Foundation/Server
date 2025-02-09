using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Organisations.Persistence.ModelConfigs;

public class OrganisationConfig : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.HasKey(p => p.OrgId);
        
        // Column Mappings
        builder.Property(o => o.OrgId).HasColumnType("uuid").HasColumnName("org_id").IsRequired();
        builder.Property(o => o.OrgName).HasColumnName("org_name").HasMaxLength(100).IsRequired();
        builder.Property(o => o.LogoUrl).HasColumnName("logo_url").HasMaxLength(500).IsRequired(false); 
        builder.Property(o => o.WebsiteUrl).HasColumnName("website_url").HasMaxLength(500).IsRequired(false); 
        builder.Property(o => o.Address).HasColumnName("address").HasMaxLength(255).IsRequired(false);
        builder.Property(o => o.Email).HasColumnName("email").HasMaxLength(100).IsRequired(false);
        builder.Property(o => o.Subscription).HasColumnName("subscription").HasMaxLength(50).IsRequired(false);
        builder.Property(o => o.Status).HasColumnType("smallint").HasColumnName("status").IsRequired();
        builder.Property(o => o.CreatedAt).HasColumnType("timestamptz").HasColumnName("created_at").IsRequired();
        builder.Property(o => o.UpdatedAt).HasColumnType("timestamptz").HasColumnName("updated_at").IsRequired();
    }
}
