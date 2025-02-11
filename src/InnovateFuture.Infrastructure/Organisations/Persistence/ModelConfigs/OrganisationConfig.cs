using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Organisations.Persistence.ModelConfigs;

public class OrganisationConfig : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.HasKey(p => p.Id);
        
        // Column Mappings
        builder.Property(o => o.Id).HasColumnType("uuid").IsRequired();
        builder.Property(o => o.OrgName).HasMaxLength(100).IsRequired();
        builder.Property(o => o.LogoUrl).HasMaxLength(500).IsRequired(false); 
        builder.Property(o => o.WebsiteUrl).HasMaxLength(500).IsRequired(false);
        builder.Property(o => o.Email).HasMaxLength(100).IsRequired(false);
        builder.Property(o => o.Subscription).HasColumnType("subscription_enum").IsRequired();
        builder.Property(o => o.OrgStatus).HasColumnType("org_status_enum").IsRequired();
        builder.Property(o => o.CreatedAt).HasColumnType("timestamp").IsRequired();
        builder.Property(o => o.UpdatedAt).HasColumnType("timestamp").IsRequired();
        builder.OwnsOne(o => o.Address, a => a.ToJson());
    }
}
