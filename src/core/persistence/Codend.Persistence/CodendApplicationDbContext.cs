using System.Reflection;
using Codend.Application.Core.Abstractions.Common;
using Codend.Application.Core.Abstractions.Data;
using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Codend.Persistence;

public abstract class CodendApplicationDbContext : DbContext, IUnitOfWork, IMigratable
{
    private readonly IDateTime _dateTime;
    private readonly IMediator _mediator;

    public abstract string Provider { get; }

    protected CodendApplicationDbContext()
    {
    }

    protected CodendApplicationDbContext(DbContextOptions options, IDateTime dateTime, IMediator mediator) :
        base(options)
    {
        _dateTime = dateTime;
        _mediator = mediator;
    }

    /// <inheritdoc /> 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Updates the specified entity entry's referenced entries.
    /// This method is recursive.
    /// </summary>
    /// <param name="entityEntry">The entity entry.</param>
    /// <param name="oldState">Entity current state.</param>
    /// <param name="newState">The state which will be set.</param>
    private static void UpdateEntityEntryReferences(EntityEntry entityEntry, EntityState oldState, EntityState newState)
    {
        if (!entityEntry.References.Any())
        {
            return;
        }

        var references = entityEntry.References.Where(r => r.TargetEntry != null && r.TargetEntry.State == oldState);
        foreach (var reference in references)
        {
            if (reference.TargetEntry == null)
            {
                continue;
            }

            reference.TargetEntry.State = newState;
            UpdateEntityEntryReferences(reference.TargetEntry, oldState, newState);
        }
    }

    /// <summary>
    /// Updates entities implementing <see cref="ISoftDeletableEntity"/> interface.
    /// </summary>
    /// <param name="time">Current date and time in UTC.</param>
    private void UpdateSoftDeletableEntities(DateTime time)
    {
        var deletedEntities = ChangeTracker
            .Entries<ISoftDeletableEntity>()
            .Where(entity => entity.State == EntityState.Deleted);

        foreach (var entityEntry in deletedEntities)
        {
            entityEntry.Property(nameof(ISoftDeletableEntity.DeletedOnUtc)).CurrentValue = time;
            entityEntry.Property(nameof(ISoftDeletableEntity.Deleted)).CurrentValue = true;
            entityEntry.State = EntityState.Modified;
            UpdateEntityEntryReferences(entityEntry, EntityState.Deleted, EntityState.Modified);
        }
    }

    /// <summary>
    /// Updates entities implementing <see cref="ICreatableEntity"/> interface.
    /// </summary>
    /// <param name="time">Current date and time in UTC.</param>
    private void UpdateCreatableEntities(DateTime time)
    {
        var addedEntities = ChangeTracker
            .Entries<ICreatableEntity>()
            .Where(entity => entity.State == EntityState.Added);

        foreach (var entityEntry in addedEntities)
        {
            entityEntry.Property(nameof(ICreatableEntity.CreatedOn)).CurrentValue = time;
        }
    }

    /// <summary>
    /// Publishes and then clears all the domain events that exist within the current transaction.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task PublishDomainEvents(CancellationToken cancellationToken)
    {
        List<EntityEntry<IAggregate>> aggregateRoots = ChangeTracker
            .Entries<IAggregate>()
            .Where(entityEntry => entityEntry.Entity.DomainEvents.Any())
            .ToList();

        List<IDomainEvent> domainEvents = aggregateRoots
            .SelectMany(entityEntry => entityEntry.Entity.DomainEvents)
            .ToList();

        aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

        IEnumerable<Task> tasks = domainEvents.Select(domainEvent => _mediator.Publish(domainEvent, cancellationToken));

        await Task.WhenAll(tasks);
    }

    /// <inheritdoc cref="DbContext.SaveChangesAsync(System.Threading.CancellationToken)" /> 
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = _dateTime.UtcNow;
        UpdateSoftDeletableEntities(utcNow);
        UpdateCreatableEntities(utcNow);

        await PublishDomainEvents(cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }
}