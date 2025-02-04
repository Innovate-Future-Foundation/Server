using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Tours.Persistence.ModelConfigs;

public class TourConfiguration : IEntityTypeConfiguration<Tour>
{
    public void Configure(EntityTypeBuilder<Tour> builder)
    {
       
        builder.HasKey(t => t.TourId);

        
        builder.HasMany(t => t.Days)
               .WithOne(d => d.Tour) 
               .HasForeignKey(d => d.TourId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.TourTemplate)
               .WithMany(tt => tt.Tours) 
               .HasForeignKey(t => t.TourTempId)
               .OnDelete(DeleteBehavior.Restrict); 

        
        builder.Property(t => t.Title)
               .IsRequired()
               .HasMaxLength(200);

       
        builder.Property(t => t.Text)
               .HasMaxLength(2000);

       
        builder.Property(t => t.Summary)
               .HasMaxLength(500);

       
        builder.ToTable("Tours");
    }
}