using Codend.Domain.Core.Abstractions;

namespace Codend.Application.Core.Abstractions.Data;

/// <summary>
/// Interface for db context entities sets.
/// </summary>
public interface IQueryableSets
{
    /// <summary>
    /// Generic db context set.
    /// </summary>
    /// <typeparam name="T">Class which inherits <see cref="IEntity{TKey}"/></typeparam>
    /// <returns>Db context set for given type.</returns>
    IQueryable<T> Queryable<T>() where T : class, IEntity;
}