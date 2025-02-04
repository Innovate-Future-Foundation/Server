
using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Days.Persistence.ModelConfigs;

public class DayConfiguration : IEntityTypeConfiguration<Day>
{
    public void Configure(EntityTypeBuilder<Day> builder)
    {
        
        builder.HasKey(d => d.DayId);

        
        builder.HasOne(d => d.Tour)
               .WithMany(t => t.Days)
               .HasForeignKey(d => d.TourId)
               .OnDelete(DeleteBehavior.Restrict); // 防止级联删除

        // 配置外键关系（DayTemplate）
        builder.HasOne(d => d.DayTemplate)
               .WithMany(dt => dt.Days) 
               .HasForeignKey(d => d.DayTemplateId)
               .OnDelete(DeleteBehavior.Restrict);

    
        builder.HasMany(d => d.Activities)
               .WithOne(a => a.Day) 
               .HasForeignKey(a => a.DayId)
               .OnDelete(DeleteBehavior.Cascade); 

   
        builder.Property(d => d.Title)
               .IsRequired()
               .HasMaxLength(200);

    
        builder.Property(d => d.Text)
               .HasMaxLength(2000);

       
        builder.Property(d => d.Summary)
               .HasMaxLength(500);

     
        builder.ToTable("Days");
    }
}