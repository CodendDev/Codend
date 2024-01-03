using Codend.Application.Core.Abstractions.Data;

namespace Codend.Api.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Applies database migrations. Returns database provider name.
    /// </summary>
    /// <returns>Database provider name.</returns>
    public static void MigrateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IMigratable>();
        dbContext.Migrate();
    }
}