using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Web.Authorization
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        readonly DefaultAuthorizationPolicyProvider FallbackPolicyProvider;

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("View", StringComparison.OrdinalIgnoreCase) ||
                policyName.StartsWith("Create", StringComparison.OrdinalIgnoreCase) ||
                policyName.StartsWith("Edit", StringComparison.OrdinalIgnoreCase) ||
                policyName.StartsWith("Delete", StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(policyName));
                return Task.FromResult(policy.Build());
            }

            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
