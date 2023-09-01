using Codend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Codend.Api;

public static class WebApplicationExtensions
{
    public static string MigrateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IMigratable>();
        dbContext.Database.Migrate();

        return dbContext.Provider;
    }
}