using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Tours.Persistence.ModelConfigs;

public class TourConfig : IEntityTypeConfiguration<Tour>
{
    public void Configure(EntityTypeBuilder<Tour> builder)
    {
        builder.HasKey(t => t.Id);
        
        // Column Mappings
        builder.Property(t => t.Id).HasColumnType("uuid").IsRequired();
        builder.Property(t => t.OrgId).HasColumnType("uuid").IsRequired();
        builder.Property(t => t.Leader).HasColumnType("uuid").IsRequired(false);
        builder.Property(a => a.Title).HasMaxLength(255).IsRequired();
        builder.Property(a => a.Comment).HasMaxLength(500).IsRequired(false); 
        builder.Property(a => a.Summary).HasMaxLength(500).IsRequired(false); 
        builder.Property(a => a.Text).HasColumnType("text").IsRequired(false); 
        builder.Property(t => t.CoverImgUrl).HasMaxLength(500).IsRequired(false);
        builder.Property(t => t.Status).HasColumnType("tour_status_enum").IsRequired();
        builder.Property(t => t.StartDate).HasColumnType("timestamptz").IsRequired();
        builder.Property(t => t.EndDate).HasColumnType("timestamptz").IsRequired();
        builder.Property(t => t.CreatedAt).HasColumnType("timestamptz").IsRequired();
        builder.Property(t => t.UpdatedAt).HasColumnType("timestamptz").IsRequired();
        
        // Relationships
        builder.HasOne(t => t.Organisation)
            .WithMany(o => o.Tours)
            .HasForeignKey(t => t.OrgId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.HasOne(t => t.LeaderProfile)
            .WithMany(p => p.LeadingTours)
            .HasForeignKey(t => t.Leader)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
