using Codend.Api.Extensions;
using Codend.Persistence.Postgres;
using Codend.Persistence.SqlServer;

namespace Codend.Api;

public static class WebApplicationExtensions
{
    public static string MigrateDatabase(this WebApplication app)
    {
        var database = "";

        try
        {
            app.MigrateDatabase<SqlServerCodendDbContext>();
            app.Logger.Log(LogLevel.Information, "Using SqlServer.");
            database = "SqlServer";
        }
        catch (InvalidOperationException)
        {
            app.MigrateDatabase<PostgresCodendDbContext>();
            app.Logger.Log(LogLevel.Information, "Using PostgreSQL.");
            database = "PostgreSQL";
        }

        return database;
    }
}