using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using InnovateFuture.Infrastructure.StudentTourEnrollments.Persistence.Interfaces;

namespace InnovateFuture.Infrastructure.StudentTourEnrollments.Persistence.Repositories;

public class StudentTourEnrollmentRepository:IStudentTourEnrollmentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public StudentTourEnrollmentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(StudentTourEnrollment studentTourEnrollment, CancellationToken cancellationToken = default)
    {
        await _dbContext.StudentTourEnrollments.AddAsync(studentTourEnrollment, cancellationToken);
    }
    
    public async Task<StudentTourEnrollment> GetByIdAsync(Guid profileId, Guid tourId,CancellationToken cancellationToken = default)
    {
        var studentTourEnrollment = await _dbContext.StudentTourEnrollments
            .Include(s=>s.Student)
            .Include(s=>s.Tour)
            .FirstOrDefaultAsync(s=>(s.ProfileId == profileId && s.TourId == tourId), cancellationToken);
       
        if (studentTourEnrollment == null)
        {
            throw new IFEntityNotFoundException("studentTourEnrollment", $"{profileId} & {tourId}");
        }
        return studentTourEnrollment;
    }
    
    public async Task UpdateAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
    public async Task<(List<StudentTourEnrollment> data, int totalItems)> GetAnyAsync(Expression<Func<StudentTourEnrollment, bool>>? predicate=null, int? limit = null, int offset=0,string? queryOrderBy=null)
    {
        IQueryable<StudentTourEnrollment> query = _dbContext.StudentTourEnrollments;
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        
        int totalItems = await query.CountAsync();
        
        if (!string.IsNullOrEmpty(queryOrderBy))
        {
            query = query.OrderBy(queryOrderBy);
        }

        var studentTourEnrollments = await query.Skip(offset)
            .Take(limit ?? totalItems).ToListAsync();
        
        return (studentTourEnrollments, totalItems);
    }
}