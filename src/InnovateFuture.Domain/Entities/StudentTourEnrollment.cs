using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Domain.Entities;

public class StudentTourEnrollment
{
    public Guid ProfileId { get; private set; }
    public Guid TourId { get; private set; }
    public DateTime EnrollmentDate { get; private set; }
    public DateTime WithdrawalDate { get; private set; }
    public EnrollmentStatusEnum Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Profile Student { get; private set; }
    public Tour Tour { get; private set; }
    
    public StudentTourEnrollment(){}

    public StudentTourEnrollment(Guid profileId,Guid tourId,DateTime enrollmentDate)
    {
        ProfileId= profileId;
        TourId= tourId;
        EnrollmentDate= enrollmentDate;
        Status = EnrollmentStatusEnum.Enrolled;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateStudentTourEnrollment(DateTime? withdrawalDate,EnrollmentStatusEnum? status)
    {
        WithdrawalDate= withdrawalDate??WithdrawalDate;
        Status = status??Status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddStudentToTourEnrollment(Profile student)
    {
        Student = student??throw new ArgumentNullException(nameof(student));
        ProfileId = student.Id;
    }

    public void AddTourToTourEnrollment(Tour tour)
    {
        Tour = tour??throw new ArgumentNullException(nameof(tour));
        TourId = tour.Id;
    }
}