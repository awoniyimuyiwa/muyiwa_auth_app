using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    /// <summary>
    /// IdentityBuilder extensions
    /// </summary>
    public static class IdentityBuilderExtensions
    {
        /// <summary>
        /// Adds implementation of identity information stores
        /// </summary>
        /// <param name="identityBuilder"></param>
        /// <returns></returns>
        public static IdentityBuilder AddInfrastructureIdentityStores(this IdentityBuilder identityBuilder)
        {
            return identityBuilder.AddEntityFrameworkStores<Data.EntityFrameworkCoreSqlServer.DbContext>();
        }
    }
}
