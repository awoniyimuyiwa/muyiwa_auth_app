using Microsoft.AspNetCore.Builder;
using Web.MiddleWares;

namespace Web.Extensions
{
    /// <summary>
    /// IApplicationBuilder extensions
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///  Set up global exception handling for /api endpoints using a middleware
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomApiExceptionHandler(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseMiddleware<ApiExceptionHandlerMiddleware>();

            return appBuilder;
        }
    }
}
