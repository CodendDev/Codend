using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Codend.Application.Core.Abstractions.Data;

/// <summary>
/// Interface for db context entities sets.
/// </summary>
public interface IDbContextSets
{
    /// <summary>
    /// Generic db context set.
    /// </summary>
    /// <typeparam name="T">Class which inherits <see cref="IEntity{TKey}"/></typeparam>
    /// <returns>Db context set for given type.</returns>
    DbSet<T> Set<T>() where T : class, IEntity;
}