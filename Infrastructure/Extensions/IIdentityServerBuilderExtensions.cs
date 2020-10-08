using IdentityServer4.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.Extensions
{
    /// <summary>
    /// IdentityServerBuilder extensions
    /// </summary>
    public static class IIdentityServerBuilderExtensions
    {
        /// <summary>
        /// Adds IdentityServer implementations of IClientStore, IResourceStore, ICorsPolicyService, IPersistedGrantStore
        /// </summary>
        /// <param name="identityServerBuilder"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddInfrastructureIdentityServerStores(this IIdentityServerBuilder identityServerBuilder, string connectionString)
        {
            var migrationAssembly = typeof(IIdentityServerBuilderExtensions).GetTypeInfo().Assembly.GetName().Name;
            
            identityServerBuilder.AddConfigurationStore(options =>
            {
                // Adds services for managing clients and resources
                options.ConfigureDbContext = builder => builder.UseSqlServer(
                    connectionString, sql => sql.MigrationsAssembly(migrationAssembly));
            }).AddOperationalStore(options => 
            {
                // Adds services for managing code, token and consent grants
                options.ConfigureDbContext = builder => builder.UseSqlServer(
                   connectionString, sql => sql.MigrationsAssembly(migrationAssembly));

                // Optional: This enables automatic token clean up
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 30;
            });

            return identityServerBuilder;
        }
    }
}
