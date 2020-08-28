﻿using Application.Services.Abstracts;
using Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Authorization
{
    class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        readonly IUserService UserService;
        readonly UserManager<User> UserManager;

        public PermissionAuthorizationHandler(IUserService userService, UserManager<User> userManager)
        {
            UserService = userService;
            UserManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null) { return; }

            //var user = await UserManager.GetUserAsync(context.User);
            var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (await UserService.UserRepository.HasPermission(userId, requirement.PermissionName))
            {
                context.Succeed(requirement);
            }

            return;
        }
    }

    class PermissionRequirement : IAuthorizationRequirement
    {
        public readonly string PermissionName;

        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }
    }
}
