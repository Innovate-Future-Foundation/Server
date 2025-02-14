using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Days.Persistence.ModelConfigs;

public class DayConfig : IEntityTypeConfiguration<Day>
{
    public void Configure(EntityTypeBuilder<Day> builder)
    {
        builder.HasKey(d => d.Id);
        
        // Column Mappings
        builder.Property(d => d.Id).HasColumnType("uuid").IsRequired();
        builder.Property(d => d.OrgId).HasColumnType("uuid").IsRequired();
        builder.Property(d => d.TourId).HasColumnType("uuid").IsRequired();
        builder.Property(a => a.Title).HasMaxLength(255).IsRequired();
        builder.Property(a => a.Comment).HasMaxLength(500).IsRequired(false); 
        builder.Property(a => a.Summary).HasMaxLength(500).IsRequired(false); 
        builder.Property(a => a.Text).HasColumnType("text").IsRequired(false); 
        builder.Property(d => d.CoverImgUrl).HasMaxLength(500).IsRequired(false);
        builder.Property(d => d.Status).HasColumnType("tour_status_enum").IsRequired();
        builder.Property(d => d.CreatedAt).HasColumnType("timestamp").IsRequired();
        builder.Property(d => d.UpdatedAt).HasColumnType("timestamp").IsRequired();
        
        // Relationships
        builder.HasOne(d => d.Organisation)
            .WithMany(u => u.Days)
            .HasForeignKey(p => p.OrgId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.HasOne(d => d.Tour)
            .WithMany(u => u.Days)
            .HasForeignKey(p => p.TourId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
