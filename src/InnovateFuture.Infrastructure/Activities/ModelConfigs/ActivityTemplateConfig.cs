using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Activities.ModelConfigs;

public class ActivityTemplateConfig : IEntityTypeConfiguration<ActivityTemplate>
{
    public void Configure(EntityTypeBuilder<ActivityTemplate> builder)
    {
        builder.ToTable("ActivityTemplates");

        builder.HasKey(at => at.ActivityTempId);

        builder.Property(at => at.ActivityTempId)
            .UseIdentityColumn();

        builder.Property(at => at.DayTempId)
            .IsRequired();

        builder.Property(at => at.OrgId)
            .IsRequired();

        builder.Property(at => at.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(at => at.Text)
            .IsRequired();

        builder.Property(at => at.Summary)
            .IsRequired();
    }
}