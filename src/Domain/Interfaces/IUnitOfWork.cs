namespace Domain.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    // TODO: implement transaction management
    // Task<object> BeginTransactionAsync(CancellationToken cancellationToken = default);
}