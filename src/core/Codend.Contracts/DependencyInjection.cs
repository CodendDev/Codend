using Microsoft.Extensions.DependencyInjection;

namespace Codend.Contracts
{
    /// <summary>
    /// DependencyInjection contracts. 
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddContracts(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddAutoMapper(assembly);

            return services;
        }
    }
}