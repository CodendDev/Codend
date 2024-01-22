using Codend.Application.Core.Abstractions.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Codend.Persistence.SqlServer;

public sealed class SqlServerCodendDbContext : CodendApplicationDbContext, IInjectableDatabase<SqlServerCodendDbContext>
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

    public override string Provider => "SqlServer";

    public static IServiceCollection AddDatabase(IServiceCollection services, string connectionString) =>
        services.AddDatabase<SqlServerCodendDbContext>(options => options.UseSqlServer(connectionString));
}