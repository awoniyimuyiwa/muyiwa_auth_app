using Microsoft.AspNetCore.Builder;
using System.Linq;

namespace Web.Extensions
{
    /// <summary>
    /// IApplicationBuilder extensions
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
       
        /// <summary>
        /// Set up localization for the app
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <param name="cultureCookieName"></param>
        public static void UseCustomLocalization(this IApplicationBuilder appBuilder, string cultureCookieName)
        {
            var supportedCultures = new[] { "en" };

            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
           
            localizationOptions.RequestCultureProviders.OfType<Microsoft.AspNetCore.Localization.CookieRequestCultureProvider>().First().CookieName = cultureCookieName;

            appBuilder.UseRequestLocalization(localizationOptions);
        }
    }
}
