using Application.Services.Abstracts;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Utils;

namespace Web.Auth
{
    class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        readonly IUserService UserService;
        public PermissionAuthorizationHandler(IUserService userService)
        {
            UserService = userService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext authorizationHandlerContext, PermissionRequirement requirement)
        {
            if (!authorizationHandlerContext.User.Identity.IsAuthenticated) { return; }

            if (authorizationHandlerContext.User.HasClaim(claim => 
            claim.Type == Constants.CustomIdentityServerScopes.Permission && 
            claim.Value == requirement.PermissionIdentifier)) 
            {
                authorizationHandlerContext.Succeed(requirement);
                return;
            }

            //var user = await UserManager.GetUserAsync(context.User);
            var userId = int.Parse(authorizationHandlerContext.User.FindFirstValue(JwtClaimTypes.Subject));
            if (await UserService.PermissionRepository.HasPermission(userId, requirement.PermissionIdentifier))
            {
                authorizationHandlerContext.Succeed(requirement);
            }

            return;
        }
    }

    class PermissionRequirement : IAuthorizationRequirement
    {
        public readonly string PermissionIdentifier;

        public PermissionRequirement(string permissionIdentifier)
        {
            PermissionIdentifier = permissionIdentifier;
        }
    }
}
