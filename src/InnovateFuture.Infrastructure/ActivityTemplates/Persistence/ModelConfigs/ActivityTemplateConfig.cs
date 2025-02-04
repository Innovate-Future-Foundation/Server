using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.ActivityTemplates.Persistence.ModelConfig;

public class ActivityTemplateConfig : IEntityTypeConfiguration<ActivityTemplate>
{
    public void Configure(EntityTypeBuilder<ActivityTemplate> builder)
    {
      
        builder.HasKey(at => at.ActivityTempId);

        builder.HasOne(at => at.DayTemplate)
               .WithMany(dt => dt.ActivityTemplates) 
               .HasForeignKey(at => at.DayTempId)
               .OnDelete(DeleteBehavior.Cascade);

       
        builder.Property(at => at.Title)
               .IsRequired()
               .HasMaxLength(200);

        
        builder.Property(at => at.Text)
               .HasMaxLength(2000);

        
        builder.Property(at => at.Summary)
               .HasMaxLength(500);

        builder.ToTable("ActivityTemplates");
    }
}
