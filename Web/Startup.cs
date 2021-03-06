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
using Infrastructure.Extensions;
using System;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using Web.Auth;

namespace Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddInfrastructureServices(connectionString)                
                .AddCustomIdentity()
                .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>()
                .AddDefaultTokenProviders()
                .AddInfrastructureIdentityStores();

            services.AddInfrastructureIdentityServices()
                .AddCustomIdentityServer()
                .AddInfrastructureIdentityServerStores(connectionString);

            services.AddInfrastructureIdentityServerServices()
                .AddApplicationServices()
                .AddWebServices()
                .AddCustomEmailSender(Configuration)
                .AddCustomAuthentication(Configuration)
                .AddCustomAuthorization()
                .AddCustomCookieOptions()
                .ConfigureCustomApplicationCookie(Configuration.GetValue<string>("AppAuthCookieName"))
                .AddCustomLocalization(Configuration.GetValue<string>("AppUserCultureCookieName"))
                .AddRazorPages(options => {
                    options.Conventions.Add(new PageRouteTransformerConvention(new SlugifyParameterTransformer()));
                });

            services.AddControllersWithViews();
             
            if (_env.IsDevelopment()) { services.AddInfrastructureDatabaseDeveloperPageExceptionFilter(); }
                
            if (string.Equals(
                Environment.GetEnvironmentVariable("ASPNETCORE_FORWARDEDHEADERS_ENABLED"),
                "true", StringComparison.OrdinalIgnoreCase))
            {
                // X-Forwarded headers from proxy servers that should be processed when app is behind a proxy server that is not IIS
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                        ForwardedHeaders.XForwardedProto;
                    // Only loopback proxies are allowed by default.
                    // Clear that restriction because forwarders are enabled by explicit configuration.
                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders()
               .UseCookiePolicy()
               .UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            if (_env.IsDevelopment())
            {
                app.UseWhen(context => !context.Request.Path.StartsWithSegments("/api"), appBuilder =>
                {
                    appBuilder.UseDeveloperExceptionPage()
                    .UseInfrastructureMigrationsEndPoint()
                    .UseStatusCodePagesWithReExecute("/status-code", "?code={0}");
                });
            } 
            else
            {
                // Use HTTP Strict Transport Security (HSTS) headers in response to browsers to tell them to only send requests over https. 
                // The default value of MaxAge is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts()
                    .UseWhen(context => !context.Request.Path.StartsWithSegments("/api"), appBuilder =>
                    {
                        appBuilder.UseExceptionHandler("/status-code")
                        .UseStatusCodePagesWithReExecute("/status-code", "?code={0}");
                    });
            }

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
            {
                appBuilder.UseCustomApiExceptionHandler();
            });
      
            app.UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                // UseIdentityServer calls UseAuthentication so no need for UseAuthentication again
                .UseIdentityServer()                 
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapRazorPages();
                    endpoints.MapControllers();
                });
        }
    }
}
