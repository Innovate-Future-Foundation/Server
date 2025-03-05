using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Interface;
using Microsoft.EntityFrameworkCore.Storage;

namespace InnovateFuture.Infrastructure.UnitOfWork.Persistence.Repositiories;

public class UnitOfWork: IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction? _currentTransaction;
    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
        {
            return _currentTransaction;
        }
        _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        return _currentTransaction;
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("No transaction started.");
        }
        
        try
        {
            await _dbContext.SaveChangesAsync(); 
            await _currentTransaction.CommitAsync();
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("No transaction started.");
        }

        try
        {
            await _currentTransaction.RollbackAsync();
        }
        finally
        {
             await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}