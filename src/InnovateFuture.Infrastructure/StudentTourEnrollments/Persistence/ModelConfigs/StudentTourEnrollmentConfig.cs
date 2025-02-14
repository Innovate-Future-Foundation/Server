using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovateFuture.Infrastructure.StudentTourEnrollments.Persistence.ModelConfigs;

public class StudentTourEnrollmentConfig : IEntityTypeConfiguration<StudentTourEnrollment>
{
    public void Configure(EntityTypeBuilder<StudentTourEnrollment> builder)
    {
        builder.HasKey(s => new { s.ProfileId, s.TourId });
        
        // Column Mappings
        builder.Property(s => s.ProfileId).HasColumnType("uuid").IsRequired();
        builder.Property(s => s.TourId).HasColumnType("uuid").IsRequired();
        builder.Property(s => s.EnrollmentDate).HasColumnType("timestamptz").IsRequired();
        builder.Property(s => s.WithdrawalDate).HasColumnType("timestamptz").IsRequired(false);
        builder.Property(s => s.Status).HasColumnType("enrollment_status_enum").IsRequired();
        builder.Property(s => s.CreatedAt).HasColumnType("timestamptz").IsRequired();
        builder.Property(s => s.UpdatedAt).HasColumnType("timestamptz").IsRequired();
        
        // Relationships
        builder.HasOne(s => s.Student)
            .WithMany(u => u.StudentTourEnrollments)
            .HasForeignKey(p => p.ProfileId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.HasOne(s => s.Tour)
            .WithMany(u => u.StudentTourEnrollments)
            .HasForeignKey(p => p.TourId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
