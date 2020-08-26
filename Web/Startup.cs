using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Web.Routing;
using Web.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Extensions;
using System;
using Microsoft.AspNetCore.HttpOverrides;
using Web.Authorization;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructureServices(Configuration.GetConnectionString("DefaultConnection"));
            services.AddApplicationServices();

            services.AddIdentity<Domain.Core.User, Domain.Core.Role>()
                .AddInfrastructureIdentityStores()
                .AddDefaultTokenProviders();
            //.AddDefaultUI();

            services.AddInfrastructureIdentityServices();

            services.AddCookieOptions()
                .AddIdentityOptions(Configuration.GetValue<string>("AppAuthCookieName"))
                .AddEmailSender(Configuration)
                .AddAuthentication()
                .AddFacebook(options => {
                    options.AppId = Configuration.GetValue<string>("AppFacebookAppId");
                    options.AppSecret = Configuration.GetValue<string>("AppFacebookAppSecret");
                    options.AccessDeniedPath = "/Login";
                })
                .AddGoogle(options => {
                    options.ClientId = Configuration.GetValue<string>("AppGoogleClientId");
                    options.ClientSecret = Configuration.GetValue<string>("AppGoogleClientSecret");
                });

            services.AddRazorPages(options => {
                options.Conventions.Add(new PageRouteTransformerConvention(new SlugifyParameterTransformer()));
            });

            // Configure authentication to be required thoroughout except for pages, controllers or actions with [AllowAnonymous] attribute
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            });

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()
                .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            // X-Forwarded headers from proxy servers that should be processed when app is behind a proxy server that is not IIS
            if (string.Equals(
                Environment.GetEnvironmentVariable("ASPNETCORE_FORWARDEDHEADERS_ENABLED"),
                "true", StringComparison.OrdinalIgnoreCase))
            {
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                        ForwardedHeaders.XForwardedProto;
                    // Only loopback proxies are allowed by default.
                    // Clear that restriction because forwarders are enabled by explicit 
                    // configuration.
                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseInfrastructureDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/status-code");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStatusCodePagesWithReExecute("/status-code", "?code={0}");

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
