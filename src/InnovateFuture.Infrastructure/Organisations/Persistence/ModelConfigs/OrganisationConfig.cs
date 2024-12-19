using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Organisations.Persistence.ModelConfigs;

public class OrganisationConfig : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.HasKey(o => o.OrgId);

        builder.Property(o => o.OrgName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(o => o.LogoUrl)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(o => o.WebsiteUrl)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(o => o.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(o => o.CreatedDate)
            .IsRequired();
    }
}