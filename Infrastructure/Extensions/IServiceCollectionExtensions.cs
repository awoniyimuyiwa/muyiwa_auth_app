using Infrastructure.Data.Abstracts;
using Infrastructure.Data.EntityFrameworkCoreSqlServer;
using Domain.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }

        public static IServiceCollection AddInfrastructureIdentityServices(this IServiceCollection services)
        {
            services.AddTransient<IUserStore<User>>(serviceProvider =>
            {
                var uow = serviceProvider.GetRequiredService<IUnitOfWork>();
                return uow.UserRepository;
            });

            services.AddTransient<IRoleStore<Role>>(serviceProvider =>
            {
                var uow = serviceProvider.GetRequiredService<IUnitOfWork>();
                return uow.RoleRepository;
            });

            return services;
        }
    }
}
