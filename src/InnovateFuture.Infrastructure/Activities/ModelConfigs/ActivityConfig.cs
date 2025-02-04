using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Activities.ModelConfigs;

public class ActivityConfig : IEntityTypeConfiguration<DomainActivity>
{
    public void Configure(EntityTypeBuilder<DomainActivity> builder)
    {
        builder.ToTable("Activities");

        builder.HasKey(a => a.ActivityId);

        builder.Property(a => a.ActivityId)
            .UseIdentityColumn();

        builder.Property(a => a.OrgId)
            .IsRequired();

        builder.Property(a => a.DayId)
            .IsRequired();

        builder.Property(a => a.TemplateId)
            .IsRequired();

        builder.Property(a => a.ActivityLead)
            .IsRequired();

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.Text)
            .IsRequired();

        builder.Property(a => a.Summary)
            .IsRequired();

        builder.Property(a => a.StartTime)
            .IsRequired();

        builder.Property(a => a.EndTime)
            .IsRequired();

        builder.Property(a => a.Status)
            .IsRequired()
            .HasMaxLength(50);
    }
}