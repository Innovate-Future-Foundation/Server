using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.Common.Persistence.Configurations;

    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
          
            builder.HasKey(a => a.ActivityId);

           
            builder.HasOne(a => a.Day)
                   .WithMany()
                   .HasForeignKey(a => a.DayId)
                   .OnDelete(DeleteBehavior.Restrict); 

           
            builder.HasOne(a => a.ActivityTemplate)
                   .WithMany()
                   .HasForeignKey(a => a.ActivityTemplateId)
                   .OnDelete(DeleteBehavior.Restrict);

          
            builder.Property(a => a.Title)
                   .IsRequired()
                   .HasMaxLength(200);

           
            builder.Property(a => a.Text)
                   .HasMaxLength(2000);

         
            builder.Property(a => a.Summary)
                   .HasMaxLength(500);

           
            builder.Property(a => a.StartTime)
                   .IsRequired();
            builder.Property(a => a.EndTime)
                   .IsRequired();

           
            builder.ToTable("Activities");
        }
    }

