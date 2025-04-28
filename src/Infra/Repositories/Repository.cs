using Infra.Data;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Infra.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DataContext _dataContext;
    private readonly DbSet<TEntity> _dbSet;
    private int _disposed;

    public Repository(DataContext dataContext)
    {
        _dataContext = dataContext;
        _dbSet = dataContext.Set<TEntity>();
    }

    public IQueryable<TEntity> Query(bool asNoTracking = true) => (asNoTracking ? _dbSet.AsNoTracking() : _dbSet).AsQueryable();

    public Task<TEntity?> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
    {
        return _dbSet.FindAsync(new object?[] { id }!, cancellationToken).AsTask();
    }

    public Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet;
        if (predicate is not null) query = query.Where(predicate);
        return query.ToListAsync(cancellationToken);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);

        return entity;
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);

        return Task.CompletedTask;
    }

    public Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _dbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken);
    }

    public Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, object updateDefinition, CancellationToken cancellationToken = default)
    {
        if (updateDefinition is not Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> expression)
        {
            throw new InvalidOperationException($"Invalid update definition for type '{typeof(TEntity).Name}'. Expected an expression of type Expression<Func<SetPropertyCalls<{typeof(TEntity).Name}>, SetPropertyCalls<{typeof(TEntity).Name}>>>.");
        }

        return _dbSet.Where(predicate).ExecuteUpdateAsync(expression, cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 0)
        {
            await _dataContext.DisposeAsync();
        }
    }
}