using Microsoft.EntityFrameworkCore.Storage;

namespace InnovateFuture.Infrastructure.UnitOfWork.Persistence.Interface;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}