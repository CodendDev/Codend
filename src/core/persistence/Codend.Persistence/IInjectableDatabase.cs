using Microsoft.Extensions.DependencyInjection;

namespace Codend.Persistence;

public interface IInjectableDatabase<TSelf>
    where TSelf : IInjectableDatabase<TSelf>
{
    static abstract IServiceCollection AddDatabase(IServiceCollection services, string connectionString);
}