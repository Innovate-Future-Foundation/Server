using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.ActivityDays.Persistence.ModelConfigs;

public class ActivityDayConfig : IEntityTypeConfiguration<ActivityDay>
{
    public void Configure(EntityTypeBuilder<ActivityDay> builder)
    {
        builder.HasKey(a => new {a.DaysBelongId, a.ActivitiesId});
        
        // Column Mappings
        builder.Property(a => a.DaysBelongId).HasColumnType("uuid").IsRequired();
        builder.Property(a => a.ActivitiesId).HasColumnType("uuid").IsRequired(); 
        
        
        builder.HasOne(a => a.Day)
            .WithMany(u => u.Activities)
            .HasForeignKey(p => p.DaysBelongId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        
        builder.HasOne(a=>a.Activity)
            .WithMany(u => u.DaysBelong)
            .HasForeignKey(p => p.ActivitiesId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
