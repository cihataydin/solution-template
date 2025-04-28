using Infra.Data;
using Domain.Interfaces;

namespace Infra.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;
    private int _disposed;

    public UnitOfWork(DataContext dataContext) => _dataContext = dataContext;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _dataContext.SaveChangesAsync(cancellationToken);

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 0)
        {
            await _dataContext.DisposeAsync();
        }
    }
}