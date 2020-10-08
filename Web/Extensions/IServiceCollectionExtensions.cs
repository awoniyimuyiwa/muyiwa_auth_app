using Domain.Core;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Web.Abstracts;
using Web.Auth;
using Web.Services;
using Web.Utils;

namespace Web.Extensions
{
    /// <summary>
    /// IServiceCollection extensions
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Add and configure authentication services for the app
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddScoped<CustomJwtBearerEvents>();

            services.AddAuthentication()
            .AddFacebook(options =>
            {
                options.AccessDeniedPath = "/login";
                options.AppId = configuration.GetValue<string>("AppFacebookAppId");
                options.AppSecret = configuration.GetValue<string>("AppFacebookAppSecret");
            })
            .AddGoogle(options =>
            {
                options.AccessDeniedPath = "/login";
                options.ClientId = configuration.GetValue<string>("AppGoogleClientId");
                options.ClientSecret = configuration.GetValue<string>("AppGoogleClientSecret");
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = configuration.GetValue<string>("AppJwtBearerAuthority");
                options.EventsType = typeof(CustomJwtBearerEvents);
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    // If the access token doesn't contain a sub claim, specify the claim to be used for NameCliamType so that ASP.NET will use it for HttpContext.User.Identity.Name
                    // NameClaimType = "name",
                    RoleClaimType = "role",
                };
            });
           
            return services;
        }

        /// <summary>
        /// Add and configure authorization services for the app
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Configure authentication to be required thoroughout except for pages, controllers or actions with [AllowAnonymous] attribute
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            });

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()
                .AddScoped<IAuthorizationHandler, ScopeAuthorizationHandler>()
               .AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();

            return services;
        }

        /// <summary>
        /// Configure Cookie settings for the app
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomCookieOptions(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.Always;
            });

            return services;
        }

        /// <summary>
        /// Add email sender for the app
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomEmailSender(this IServiceCollection services, IConfiguration configuration)
        {
            // services.Configure<Utils.SendGridOptions>(configuration.GetSection("SendGrid"));
            // Load SendGridOptions from appSettings.json
            services.Configure<SendGridOptions>(configuration);
            services.AddTransient<IEmailSender, SendGridEmailSender>();

            return services;
        }

        /// <summary>
        /// Add and configure identity services for the app
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IdentityBuilder AddCustomIdentity(this IServiceCollection services)
        {
            return services.AddIdentity<User, Role>(options =>
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
        }

        /// <summary>
        /// Add and configure identity server services for the app
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddCustomIdentityServer(this IServiceCollection services)
        {
            var identityServerBuilder = services.AddIdentityServer(options =>
            {
                options.UserInteraction.ConsentUrl = "/consent";
                options.UserInteraction.DeviceVerificationUrl = "/device-verification";
                options.UserInteraction.ErrorUrl = "/error";
                options.UserInteraction.LoginUrl = "/login";
                options.UserInteraction.LogoutUrl = "/logout";
            })
            .AddAspNetIdentity<User>();
 
            if (!string.Equals(
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                "Production", StringComparison.OrdinalIgnoreCase))
            {
                // Not recommended for production - you need to store your key material somewhere secure
                identityServerBuilder.AddDeveloperSigningCredential();
            }

            return identityServerBuilder;
        }

        /// <summary>
        /// Add and configure localization services for the app
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cultureCookieName"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomLocalization(this IServiceCollection services, string cultureCookieName)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources")
                .Configure<RequestLocalizationOptions>(options =>
                {
                    var cultures = new[]
                    {
                        new CultureInfo("en"),
                    };
                    options.DefaultRequestCulture = new RequestCulture("en");
                    options.SupportedCultures = cultures;
                    options.SupportedUICultures = cultures;
                    options.RequestCultureProviders.OfType<CookieRequestCultureProvider>().First().CookieName = cultureCookieName;

                    options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(context =>
                    {
                        var result = new ProviderCultureResult("en");
                        return Task.FromResult(result);
                    }));
                });

            return services;
        }

        /// <summary>
        /// Add services in this layer
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddScoped<IRepositorySeeder, RepositorySeeder>()
                .AddScoped<IProfileService, CustomIdentityServerProfileService>();

            return services;
        }

        /// <summary>
        /// Add and configure auth cookie for the aapp
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appAuthCookieName"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureCustomApplicationCookie(this IServiceCollection services, string appAuthCookieName)
        {
            services.AddScoped<CustomCookieAuthenticationEvents>();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.Name = appAuthCookieName;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.EventsType = typeof(CustomCookieAuthenticationEvents);
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
                //options.SlidingExpiration = true;
            });

            return services;
        }
    }
}
