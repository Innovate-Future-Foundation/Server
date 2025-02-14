using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.StudentTourEnrollments.Persistence.Interfaces;

public interface IStudentTourEnrollmentRepository
{
    Task AddAsync(StudentTourEnrollment studentTourEnrollment, CancellationToken cancellationToken = default);
    Task<StudentTourEnrollment> GetByIdAsync(Guid profileId, Guid tourId, CancellationToken cancellationToken = default);
    Task UpdateAsync();
    Task<(List<StudentTourEnrollment> data, int totalItems)> GetAnyAsync(
        Expression<Func<StudentTourEnrollment, bool>>? predicate=null, int? limit = null, int offset=0, string? queryOrderBy=null);
}