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
        builder.Property(t => t.Title).HasMaxLength(100).IsRequired();
        builder.Property(t => t.OrgId).HasColumnType("uuid").HasMaxLength(500).IsRequired();
        builder.Property(t => t.Leader).HasColumnType("uuid").HasMaxLength(500).IsRequired(false);
        builder.Property(t => t.Description).HasMaxLength(500).IsRequired(false); 
        builder.Property(t => t.CoverImgUrl).HasMaxLength(500).IsRequired(false);
        builder.Property(t => t.Status).HasColumnType("tour_status_enum").IsRequired();
        builder.Property(t => t.StartDate).HasColumnType("timestamptz").IsRequired();
        builder.Property(t => t.EndDate).HasColumnType("timestamptz").IsRequired();
        builder.Property(t => t.CreatedAt).HasColumnType("timestamptz").IsRequired();
        builder.Property(t => t.UpdatedAt).HasColumnType("timestamptz").IsRequired();
        
        // Relationships
        builder.HasOne(t => t.Organisation)
            .WithMany(u => u.Tours)
            .HasForeignKey(p => p.OrgId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.HasOne(t => t.LeaderProfile)
            .WithMany(u => u.LeadingTours)
            .HasForeignKey(p => p.Leader)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired();
    }
}
