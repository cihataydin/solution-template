using Infra.Data;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Infra.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
{
    private readonly DataContext _dataContext;
    private readonly DbSet<TEntity> _dbSet;
    private int _disposed;

    public Repository(DataContext dataContext)
    {
        _dataContext = dataContext;
        _dbSet = dataContext.Set<TEntity>();
    }

    public IQueryable<TEntity> Query(bool asNoTracking = true) =>
        asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();

    public Task<TEntity?> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
        => _dbSet.FindAsync(new object?[] { id }!, cancellationToken).AsTask();

    public Task<List<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = Query(asNoTracking);
        if (predicate != null) query = query.Where(predicate);
        return query.ToListAsync(cancellationToken);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => _dbSet.AnyAsync(predicate, cancellationToken);

    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        => predicate == null
            ? _dbSet.CountAsync(cancellationToken)
            : _dbSet.CountAsync(predicate, cancellationToken);

    public TEntity Add(TEntity entity)
        => _dbSet.Add(entity).Entity;

    public async Task<TEntity> AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
        => (await _dbSet.AddAsync(entity, cancellationToken)).Entity;

    public void AddRange(params TEntity[] entities)
        => _dbSet.AddRange(entities);

    public void AddRange(IEnumerable<TEntity> entities)
        => _dbSet.AddRange(entities);

    public Task AddRangeAsync(params TEntity[] entities)
        => _dbSet.AddRangeAsync(entities);

    public Task AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
        => _dbSet.AddRangeAsync(entities, cancellationToken);

    public TEntity Attach(TEntity entity)
        => _dbSet.Attach(entity).Entity;

    public void AttachRange(params TEntity[] entities)
        => _dbSet.AttachRange(entities);

    public void AttachRange(IEnumerable<TEntity> entities)
        => _dbSet.AttachRange(entities);

    public TEntity Update(TEntity entity)
        => _dbSet.Update(entity).Entity;

    public void UpdateRange(params TEntity[] entities)
        => _dbSet.UpdateRange(entities);

    public void UpdateRange(IEnumerable<TEntity> entities)
        => _dbSet.UpdateRange(entities);

    public TEntity Remove(TEntity entity)
        => _dbSet.Remove(entity).Entity;

    public void RemoveRange(params TEntity[] entities)
        => _dbSet.RemoveRange(entities);

    public void RemoveRange(IEnumerable<TEntity> entities)
        => _dbSet.RemoveRange(entities);

    public Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => _dbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken);

    public Task<int> ExecuteUpdateAsync(
        Expression<Func<TEntity, bool>> predicate,
        object updateDefinition,
        CancellationToken cancellationToken = default)
    {
        if (updateDefinition is not Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> updateExpression)
        {
            throw new ArgumentException("Invalid update expression for EF Core", nameof(updateDefinition));
        }

        return _dbSet.Where(predicate).ExecuteUpdateAsync(updateExpression, cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _dataContext.SaveChangesAsync(cancellationToken);

    public object Property(TEntity entity, string propertyName)
    {
        return EF.Property<object>(entity, propertyName);
    }

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 0)
        {
            await _dataContext.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}
