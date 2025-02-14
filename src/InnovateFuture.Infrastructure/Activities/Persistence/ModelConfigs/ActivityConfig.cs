using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Activities.Persistence.ModelConfigs;

public class ActivityConfig : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(a => a.Id);
        
        // Column Mappings
        builder.Property(a => a.Id).HasColumnType("uuid").IsRequired();
        builder.Property(a => a.OrgId).HasColumnType("uuid").IsRequired(); 
        builder.Property(a => a.Title).HasMaxLength(255).IsRequired();
        builder.Property(a => a.Comment).HasMaxLength(500).IsRequired(false); 
        builder.Property(a => a.Summary).HasMaxLength(500).IsRequired(false); 
        builder.Property(a => a.Text).HasColumnType("text").IsRequired(false); 
        builder.Property(a => a.Location).HasMaxLength(100).IsRequired(false);
        builder.Property(a => a.CoverImgUrl).HasMaxLength(500).IsRequired(false);
        builder.Property(a => a.Status).HasColumnType("tour_status_enum").IsRequired();
        builder.Property(a => a.StartTime).HasColumnType("timestamptz").IsRequired();
        builder.Property(a => a.EndTime).HasColumnType("timestamptz").IsRequired();
        builder.Property(a => a.CreatedAt).HasColumnType("timestamptz").IsRequired();
        builder.Property(a => a.UpdatedAt).HasColumnType("timestamptz").IsRequired();
        
        // Relationships
        builder.HasOne(a => a.Organisation)
            .WithMany(u => u.Activities)
            .HasForeignKey(p => p.OrgId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
