using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;

public interface IOrgRepository
{
    Task AddAsync(Organisation organisation, CancellationToken cancellationToken = default);
    
    
    Task<Organisation> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync();

    Task<(List<Organisation> data, int totalItems)> GetAnyAsync(
        Expression<Func<Organisation, bool>>? predicate=null, int? limit = null, int offset=0, string? queryOrderBy=null);
}