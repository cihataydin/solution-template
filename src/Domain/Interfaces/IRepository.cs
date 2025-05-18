using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IRepository<TEntity> : IAsyncDisposable where TEntity : class
{
    IQueryable<TEntity> Query(bool asNoTracking = true);

    Task<TEntity?> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default);

    Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? predicate = null, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

    TEntity Add(TEntity entity);
    Task<TEntity> AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    void AddRange(params TEntity[] entities);

    void AddRange(IEnumerable<TEntity> entities);

    Task AddRangeAsync(params TEntity[] entities);

    Task AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);

    TEntity Attach(TEntity entity);
    void AttachRange(params TEntity[] entities);

    void AttachRange(IEnumerable<TEntity> entities);

    TEntity Update(TEntity entity);

    void UpdateRange(params TEntity[] entities);

    void UpdateRange(IEnumerable<TEntity> entities);

    TEntity Remove(TEntity entity);

    void RemoveRange(params TEntity[] entities);

    void RemoveRange(IEnumerable<TEntity> entities);

    Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<int> ExecuteUpdateAsync(
    Expression<Func<TEntity, bool>> predicate,
    object updateDefinition,
    CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}