using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.DayTemplates.Persistence.ModelConfigs;

public class DayTemplateConfig : IEntityTypeConfiguration<DayTemplate>
{
    public void Configure(EntityTypeBuilder<DayTemplate> builder)
    {
        
        builder.HasKey(dt => dt.DayTempId);

        
        builder.HasOne(dt => dt.TourTemplate)
               .WithMany(tt => tt.DayTemplates) 
               .HasForeignKey(dt => dt.TourTempId)
               .OnDelete(DeleteBehavior.Restrict); 

      
        builder.HasMany(dt => dt.ActivityTemplates)
               .WithOne(at => at.DayTemplate)
               .HasForeignKey(at => at.DayTempId)
               .OnDelete(DeleteBehavior.Cascade); 

      
        builder.Property(dt => dt.Title)
               .IsRequired()
               .HasMaxLength(200);

       
        builder.Property(dt => dt.Text)
               .HasMaxLength(2000);

     
        builder.Property(dt => dt.Summary)
               .HasMaxLength(500);
        
      
        builder.ToTable("DayTemplates");
    }
}
