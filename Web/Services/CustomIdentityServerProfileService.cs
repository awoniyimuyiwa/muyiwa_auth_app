using Application.Services.Abstracts;
using Domain.Core;
using Domain.Core.Dtos;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Utils;

namespace Web.Services
{
    class CustomIdentityServerProfileService : IProfileService
    {
        readonly UserManager<User> UserManager;
        readonly IUserClaimsPrincipalFactory<User> UserClaimsPrincipalFactory;
        readonly IUserService UserService;

        public CustomIdentityServerProfileService(
            UserManager<User> userManager,
            IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,
            IUserService userService)
        {
            UserManager = userManager;
            UserClaimsPrincipalFactory = userClaimsPrincipalFactory;
            UserService = userService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subClaim = context.Subject.FindFirst(claim => claim.Type == JwtClaimTypes.Subject);
            if (subClaim == null) { return; }

            var user = await UserManager.FindByIdAsync(subClaim.Value);
            if (user == null) { return; }

            var principal = await UserClaimsPrincipalFactory.CreateAsync(user);
            var principalClaims = principal.Claims.ToList();
            var filteredPrincipalClaims = principalClaims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            if (context.RequestedClaimTypes.Contains(Constants.CustomIdentityServerScopes.Permission))
            {
                var permissionDtos = await UserService.PermissionRepository.GetAllForUserAsDto(user.Id);

                foreach (PermissionDto permissionDto in permissionDtos)
                {
                    filteredPrincipalClaims.Add(new Claim(Constants.CustomIdentityServerScopes.Permission, permissionDto.Name));
                }
            }

            context.IssuedClaims = filteredPrincipalClaims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var isActive = false;

            var subClaim = context.Subject.FindFirst(claim => claim.Type == JwtClaimTypes.Subject);
            if (subClaim != null) 
            {
                var user = await UserManager.FindByIdAsync(subClaim.Value);
                isActive = user != null;
            }
           
            context.IsActive = isActive;
        }
    }
}
