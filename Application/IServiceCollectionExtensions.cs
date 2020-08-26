using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // One instance of a service class per request
            services.AddScoped<Services.Abstracts.IPermissionService, Services.PermissionService>();
            services.AddScoped<Services.Abstracts.IRepositorySeeder, Services.RepositorySeeder>();
            services.AddScoped<Services.Abstracts.IRoleService, Services.RoleService>();            
            services.AddScoped<Services.Abstracts.IUserService, Services.UserService>();

            return services;
        }
    }
}
