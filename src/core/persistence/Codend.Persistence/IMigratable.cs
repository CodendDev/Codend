using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Codend.Persistence;

public interface IMigratable
{
    DatabaseFacade Database { get; }

    string Provider { get; }
}