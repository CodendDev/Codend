using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Codend.Persistence.Repositories;

public abstract class GenericRepository<TKey, TKeyPrimitive, TEntity>
    where TKeyPrimitive : IComparable
    where TKey : IEntityId<TKeyPrimitive>
    where TEntity : Entity<TKey>
{
    protected readonly CodendApplicationDbContext Context;

    protected GenericRepository(CodendApplicationDbContext context)
    {
        Context = context;
    }

    public void Add(TEntity entity)
    {
        Context.Add(entity);
    }

    public Task<TEntity?> GetByIdAsync(TKey entityId, CancellationToken cancellationToken)
    {
        var entity = Context
            .Set<TEntity>()
            .SingleOrDefaultAsync(entity => Equals(entity.Id, entityId), cancellationToken: cancellationToken);

        return entity;
    }

    public Task<TEntity?> GetByIdAsync(TKey entityId)
    {
        var entity = Context
            .Set<TEntity>()
            .SingleOrDefaultAsync(entity => Equals(entity.Id, entityId));

        return entity;
    }

    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().UpdateRange(entities);
    }

    public Task<bool> Exists(TKey key)
    {
        return Context.Set<TEntity>()
            .AnyAsync(e => Equals(e.Id, key));
    }
}