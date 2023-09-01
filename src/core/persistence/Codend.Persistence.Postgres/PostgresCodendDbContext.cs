using Codend.Application.Core.Abstractions.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codend.Persistence.Postgres;

public sealed class PostgresCodendDbContext : CodendApplicationDbContext
{
    public PostgresCodendDbContext()
    {
    }

    public PostgresCodendDbContext(DbContextOptions options, IDateTime dateTime, IMediator mediator)
        : base(options, dateTime, mediator)
    {
    }

    /// <inheritdoc /> 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql();
    }
}