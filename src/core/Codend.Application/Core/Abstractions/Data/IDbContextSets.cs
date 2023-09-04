using Codend.Domain.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Codend.Application.Core.Abstractions.Data;

public interface IDbContextSets
{
    DbSet<T> Set<T>() where T : class, IEntity;
}