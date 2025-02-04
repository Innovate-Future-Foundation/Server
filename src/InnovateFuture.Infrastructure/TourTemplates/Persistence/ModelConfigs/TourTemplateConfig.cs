using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnovateFuture.Infrastructure.TourTemplates.Persistence.ModelConfigs;

public class TourTemplateConfig : IEntityTypeConfiguration<TourTemplate>
{
    public void Configure(EntityTypeBuilder<TourTemplate> builder)
    {
    
        builder.HasKey(tt => tt.TourTempId);

       
        builder.HasMany(tt => tt.DayTemplates)
               .WithOne(dt => dt.TourTemplate) 
               .HasForeignKey(dt => dt.TourTempId)
               .OnDelete(DeleteBehavior.Cascade); 

        builder.HasMany(tt => tt.Tours)
               .WithOne(t => t.TourTemplate)
               .HasForeignKey(t => t.TourTempId)
               .OnDelete(DeleteBehavior.Restrict); 

    
        builder.Property(tt => tt.Title)
               .IsRequired()
               .HasMaxLength(200);

     
        builder.Property(tt => tt.Text)
               .HasMaxLength(2000);

        builder.Property(tt => tt.Summary)
               .HasMaxLength(500);

        builder.ToTable("TourTemplates");
    }
}