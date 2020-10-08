using Domain.Core;
using IdentityServer4.Models;
using Infrastructure.Data.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Abstracts;
using Web.Utils;

namespace Web.Services
{
    class RepositorySeeder : IRepositorySeeder
    {
        readonly IUnitOfWork Uow;
        readonly IIdentityServerConfigurationUnitOfWork IdentityServerConfigurationUow;
        readonly UserManager<User> UserManager;
        readonly IConfiguration Configuration;

        public RepositorySeeder(
            IUnitOfWork uow,
            IIdentityServerConfigurationUnitOfWork identityServerConfigurationUow,
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            Uow = uow;
            IdentityServerConfigurationUow = identityServerConfigurationUow;
            UserManager = userManager;
            Configuration = configuration;
        }

        public async Task Seed()
        {
            await SeedPermissions();
            await SeedRoles();
            await SeedAdmins();

            await SeedApiScopes();
            await SeedApiResources();
            await SeedIdentityResources();
            await SeedClients();
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

                // Permissions for admin operations on todo-item microservice
                new Permission { Name = Constants.PermissionNames.ViewTodoItem },

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

        private async Task SeedAdmins()
        {           
            if (!await Uow.UserRepository.IsEmpty()) { return; }

            string defaultAdminPassword = Configuration.GetValue<string>("AppDefaultAdminPassword");
            if (defaultAdminPassword == null) { throw new Exception("AppDefaultAdminPassword configuration is not set in appsettings.json"); }

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

        private async Task SeedApiScopes()
        {
            if (!await IdentityServerConfigurationUow.CustomResourceStore.IsApiScopesEmpty()) { return; }

            var apiScopes = IdentityServerConfig.ApiScopes();

            List<Task> tasks = new List<Task>();
            foreach (ApiScope apiScope in apiScopes)
            {
                tasks.Add(IdentityServerConfigurationUow.CustomResourceStore.Add(apiScope));
            }

            await Task.WhenAll(tasks);
            await IdentityServerConfigurationUow.Commit();
        }

        private async Task SeedApiResources()
        {
            if (!await IdentityServerConfigurationUow.CustomResourceStore.IsApiResourcesEmpty()) { return; }

            var apiResources = IdentityServerConfig.ApiResources();

            List<Task> tasks = new List<Task>();
            foreach (ApiResource apiResource in apiResources)
            {
                tasks.Add(IdentityServerConfigurationUow.CustomResourceStore.Add(apiResource));
            }

            await Task.WhenAll(tasks);
            await IdentityServerConfigurationUow.Commit();
        }

        private async Task SeedIdentityResources()
        {
            if (!await IdentityServerConfigurationUow.CustomResourceStore.IsIdentityResourcesEmpty()) { return; }

            var identityResources = IdentityServerConfig.IdentityResources();

            List<Task> tasks = new List<Task>();
            foreach (IdentityResource identityResource in identityResources)
            {
                tasks.Add(IdentityServerConfigurationUow.CustomResourceStore.Add(identityResource));
            }

            await Task.WhenAll(tasks);
            await IdentityServerConfigurationUow.Commit();
        }

        private async Task SeedClients()
        {
            if (!await IdentityServerConfigurationUow.CustomClientStore.IsEmpty()) { return; }

            var todoAppSecret = Configuration.GetValue<string>("TodoAppSecret");
            if (todoAppSecret == null) { throw new Exception("TodoAppSecret is not set in appsettings.json"); }

            var todoAppUrl = Configuration.GetValue<string>("TodoAppUrl");
            if (todoAppUrl == null) { throw new Exception("TodoAppUrl is not set in appsettings.json"); }

            // Get clients for a sample todo app microservice
            var todoAppClient = IdentityServerConfig.TodoAppClient(todoAppSecret, todoAppUrl);

            await IdentityServerConfigurationUow.CustomClientStore.Add(todoAppClient);
            await IdentityServerConfigurationUow.Commit();
        }
    }
}