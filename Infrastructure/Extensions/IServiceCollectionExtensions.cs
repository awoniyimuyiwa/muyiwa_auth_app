using Infrastructure.Data.Abstracts;
using Infrastructure.Data.EntityFrameworkCoreSqlServer;
using Domain.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.Stores;

namespace Infrastructure.Extensions
{
    /// <summary>
    /// IServiceCollection extensions
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            // AddBbContext adds dbConetxt as scoped i.e one per request
            services.AddDbContext<Data.EntityFrameworkCoreSqlServer.DbContext>(
                options => options.UseSqlServer(connectionString).UseLazyLoadingProxies());

            // One instance of UnitOfWork per request
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIdentityServerConfigurationUnitOfWork, IdentityServerConfigurationUnitOfWork>();

            return services;
        }

        public static IServiceCollection AddInfrastructureIdentityServices(this IServiceCollection services)
        {
            services.AddTransient<IUserStore<User>>(serviceProvider =>
            {
                var uow = serviceProvider.GetRequiredService<IUnitOfWork>();
                return uow.UserRepository;
            })
            .AddTransient<IRoleStore<Role>>(serviceProvider =>
            {
                var uow = serviceProvider.GetRequiredService<IUnitOfWork>();
                return uow.RoleRepository;
            });

            return services;
        }

        public static IServiceCollection AddInfrastructureIdentityServerServices(this IServiceCollection services)
        {
            services.AddTransient<IClientStore>(serviceProvider =>
            {
                var uow = serviceProvider.GetRequiredService<IIdentityServerConfigurationUnitOfWork>();
                return uow.CustomClientStore;
            })
            .AddTransient<IResourceStore>(serviceProvider =>
            {
                var uow = serviceProvider.GetRequiredService<IIdentityServerConfigurationUnitOfWork>();
                return uow.CustomResourceStore;
            });

            return services;
        }
    }
}
