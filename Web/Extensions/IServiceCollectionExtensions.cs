using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Web.Extensions
{
    /// <summary>
    /// IServiceCollection extensions
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Add email sender for the app
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
        {
            // services.Configure<Utils.SendGridOptions>(configuration.GetSection("SendGrid"));
            // Load SendGridOptions from appSettings.json
            services.Configure<Utils.SendGridOptions>(configuration);
            services.AddTransient<IEmailSender, Utils.SendGridEmailSender>();

            return services;
        }

        /// <summary>
        /// Configure Cookie settings for the app
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCookieOptions(this IServiceCollection services)
        {
            services.Configure<Microsoft.AspNetCore.Builder.CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
                options.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
            });

            return services;
        }

        /// <summary>
        /// Configure Identity for the app
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appAuthCookieName"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityOptions(this IServiceCollection services, string appAuthCookieName)
        {
            services.Configure<Microsoft.AspNetCore.Identity.IdentityOptions>(options =>
            {
                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                // SignIn settings.
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;

                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.Name = appAuthCookieName;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);

                options.LoginPath = "/login";
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                };
                //options.SlidingExpiration = true;
            });

            return services;
        }
    }
}
