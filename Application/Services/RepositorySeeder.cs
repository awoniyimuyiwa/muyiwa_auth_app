using Application.Services.Abstracts;
using Domain.Core;
using Infrastructure.Data.Abstracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    class RepositorySeeder : IRepositorySeeder
    {
        public RepositorySeeder(
            IUnitOfWork uow, UserManager<User> userManager)
        {
            Uow = uow;
            UserManager = userManager;
        }

        readonly IUnitOfWork Uow;
        readonly UserManager<User> UserManager;

        public async Task Seed(string defaultAdminPassword)
        {
            await SeedPermissions();
            await SeedRoles();
            await SeedAdmins(defaultAdminPassword);
        }

        private async Task SeedPermissions()
        {
            if (!await Uow.PermissionRepository.IsEmpty()) { return; }

            var permissions = new Permission[]
            {
                new Permission { Name = Constants.PermissionNames.CreatePermission },
                new Permission { Name = Constants.PermissionNames.ViewPermission },
                new Permission { Name = Constants.PermissionNames.EditPermission },
                new Permission { Name = Constants.PermissionNames.DeletePermission },

                new Permission { Name = Constants.PermissionNames.CreateRole },
                new Permission { Name = Constants.PermissionNames.ViewRole },
                new Permission { Name = Constants.PermissionNames.EditRole },
                new Permission { Name = Constants.PermissionNames.DeleteRole },

                new Permission { Name = Constants.PermissionNames.CreateUser },
                new Permission { Name = Constants.PermissionNames.ViewUser },
                new Permission { Name = Constants.PermissionNames.EditUser },
                new Permission { Name = Constants.PermissionNames.DeleteUser },
            };

            List<Task<Permission>> tasks = new List<Task<Permission>>();
            foreach (Permission permission in permissions)
            {
                tasks.Add(Uow.PermissionRepository.Add(permission));
            }

            await Task.WhenAll(tasks);
            await Uow.Commit();
        }

        private async Task SeedRoles()
        {
            if (!await Uow.RoleRepository.IsEmpty()) { return; }

            var adminRole = new Role
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "This role has all permissions"
            };

            var permissions = await Uow.PermissionRepository.GetAll();

            foreach (Permission permission in permissions)
            {
                adminRole.AddPermissions(permission);
            }

            await Uow.RoleRepository.Add(adminRole);
            await Uow.Commit();
        }

        private async Task SeedAdmins(string defaultAdminPassword)
        {
            if (!await Uow.UserRepository.IsEmpty()) { return; }

            var user = new User
            {
                UserName = "Muyiwa",
                Email = "muyiwaawoniyi@yahoo.com",
                EmailConfirmed = true,
                Profile = new Profile
                {
                    FirstName = "Muyiwa",
                    LastName = "Awoniyi"
                }
            };

            var adminRole = await Uow.RoleRepository.FindOneBy(role => role.NormalizedName == "ADMIN");
            user.AddRoles(adminRole);

            IdentityResult result = await UserManager.CreateAsync(user, defaultAdminPassword);
            if (!result.Succeeded)
            {
                throw new Exception($"An error occured while seeding admins, user creation with UserManager failed. Reason: {result.Errors.First()}");
            }

            await Uow.Commit();
        }
    }
}