using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Domain.Entities;

public class StudentTourEnrollment
{
    public Guid ProfileId { get; private set; }
    public Guid TourId { get; private set; }
    public DateTime EnrollmentDate { get; private set; }
    public DateTime WithdrawalDate { get; private set; }
    public EnrollmentStatusEnum Status { get; private set; }
}