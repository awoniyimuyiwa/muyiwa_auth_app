using Domain.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Auth
{
    class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
    {
        public CustomUserClaimsPrincipalFactory(
            UserManager<User> userManager, 
            RoleManager<Role> roleManager, 
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options) {}

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            // Add claims to identity here. The claims added here will be stored in the auth cookie.
            // They should only be claims that won't impact security when their values are updated in the database

            return identity;
        }
    }
}
