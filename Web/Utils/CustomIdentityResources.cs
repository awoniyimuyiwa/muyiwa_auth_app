using IdentityServer4.Models;

namespace Web.Utils
{
    internal static class CustomIdentityResources
    {
        internal class Permission : IdentityResource
        {
            internal Permission() : base()
            {
                Name = Constants.CustomIdentityServerScopes.Permission;
                DisplayName = "Permission";

                UserClaims.Add(ClaimTypes.Permission);
            }

            internal static class ClaimTypes
            {
                internal const string Permission = "permission";
            }
        }
    }
}
