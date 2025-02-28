using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.ActivityProfiles.Persistence.ModelConfigs;

public class ActivityProfileConfig : IEntityTypeConfiguration<ActivityProfile>
{
    public void Configure(EntityTypeBuilder<ActivityProfile> builder)
    {
        builder.HasKey(a => new {a.AssignedActivitiesId, a.TeachersAssignedId});
        
        // Column Mappings
        builder.Property(a => a.AssignedActivitiesId).HasColumnType("uuid").IsRequired();
        builder.Property(a => a.TeachersAssignedId).HasColumnType("uuid").IsRequired(); 
        
        
        builder.HasOne(a => a.Teacher)
            .WithMany(u => u.AssignedActivities)
            .HasForeignKey(p => p.TeachersAssignedId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        
        builder.HasOne(a=>a.Activity)
            .WithMany(u => u.TeachersAssigned)
            .HasForeignKey(p => p.AssignedActivitiesId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
