using Codend.Application.Core.Abstractions.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codend.Persistence.SqlServer;

public sealed class SqlServerCodendDbContext : CodendApplicationDbContext
{
    public SqlServerCodendDbContext()
    {
    }

    public SqlServerCodendDbContext(DbContextOptions options, IDateTime dateTime, IMediator mediator)
        : base(options, dateTime, mediator)
    {
    }

    /// <inheritdoc /> 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer();
    }
}