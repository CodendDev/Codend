using Codend.Application.Core.Abstractions.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Codend.Persistence.Postgres;

public sealed class PostgresCodendDbContext : CodendApplicationDbContext, IInjectableDatabase<PostgresCodendDbContext>
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

    public override string Provider => "PostgreSQL";

    public static IServiceCollection AddDatabase(IServiceCollection services, string connectionString) =>
        services.AddDatabase<PostgresCodendDbContext>(options => options.UseNpgsql(connectionString));
}