using System.Reflection;
using Codend.Application.Core.Abstractions.Common;
using Codend.Application.Core.Abstractions.Data;
using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Codend.Persistence;

public sealed class CodendApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly IDateTime _dateTime;
    private readonly IMediator _mediator;

    public CodendApplicationDbContext()
    {
    }

    public CodendApplicationDbContext(DbContextOptions options, IDateTime dateTime, IMediator mediator) : base(options)
    {
        _dateTime = dateTime;
        _mediator = mediator;
    }

    /// <inheritdoc /> 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer();
    }

    /// <inheritdoc /> 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Updates the specified entity entry's referenced entries in the deleted state to the modified state.
    /// This method is recursive.
    /// </summary>
    /// <param name="entityEntry">The entity entry.</param>
    private static void UpdateDeletedEntityEntryReferencesToUnchanged(EntityEntry entityEntry)
    {
        if (!entityEntry.References.Any())
        {
            return;
        }

        var references = entityEntry.References.Where(r => r.TargetEntry.State == EntityState.Deleted);
        foreach (ReferenceEntry referenceEntry in references)
        {
            referenceEntry.TargetEntry.State = EntityState.Unchanged;

            UpdateDeletedEntityEntryReferencesToUnchanged(referenceEntry.TargetEntry);
        }
    }

    /// <summary>
    /// Updates the entities implementing <see cref="ISoftDeletableEntity"/> interface.
    /// </summary>
    /// <param name="time">The current date and time in UTC format.</param>
    private void UpdateSoftDeletableEntities(DateTime time)
    {
        foreach (EntityEntry<ISoftDeletableEntity> entityEntry in ChangeTracker.Entries<ISoftDeletableEntity>())
        {
            if (entityEntry.State != EntityState.Deleted)
            {
                continue;
            }

            entityEntry.Property(nameof(ISoftDeletableEntity.DeletedOnUtc)).CurrentValue = time;

            entityEntry.Property(nameof(ISoftDeletableEntity.Deleted)).CurrentValue = true;

            entityEntry.State = EntityState.Modified;

            UpdateDeletedEntityEntryReferencesToUnchanged(entityEntry);
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

        List<DomainEvent> domainEvents = aggregateRoots
            .SelectMany(entityEntry => entityEntry.Entity.DomainEvents)
            .ToList();

        aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

        IEnumerable<Task> tasks = domainEvents.Select(domainEvent => _mediator.Publish(domainEvent, cancellationToken));

        await Task.WhenAll(tasks);
    }

    /// <inheritdoc /> 
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = _dateTime.UtcNow;
        UpdateSoftDeletableEntities(utcNow);

        await PublishDomainEvents(cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }
}