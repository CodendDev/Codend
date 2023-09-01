using Codend.Application.Core.Abstractions.Common;
using Codend.Application.Core.Abstractions.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codend.Persistence;

public sealed class SqlServerCodendDbContext : CodendApplicationDbContext, IUnitOfWork
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