using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Web.Utils
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources() => new List<IdentityResource>    
        {
           new IdentityResources.Address(),
           new IdentityResources.Email(),
           new IdentityResources.OpenId(),
           new IdentityResources.Phone(),
           new IdentityResources.Profile(),

           new CustomIdentityResources.Permission()
        };

        public static IEnumerable<ApiScope> ApiScopes() => new List<ApiScope>    
        {
            new ApiScope(Constants.CustomIdentityServerScopes.DeleteTodoItem, "Permission to delete your todo-items"),
            new ApiScope(Constants.CustomIdentityServerScopes.ReadTodoItem, "Permission to read your todo-items"),
            
            // worker.todoitem Should only be assigned to server to server confidential clients not front-end clients
            new ApiScope(Constants.CustomIdentityServerScopes.WorkerTodoItem, "Permission to read all todo-items"), 
            
            new ApiScope(Constants.CustomIdentityServerScopes.WriteTodoItem, "Permission to write your todo-items")
        };

        public static IEnumerable<ApiResource> ApiResources() => new List<ApiResource>
        {
            new ApiResource("todo-item", "Todo-item API")
            {
                Scopes = 
                {
                    Constants.CustomIdentityServerScopes.DeleteTodoItem,
                    Constants.CustomIdentityServerScopes.ReadTodoItem,
                     Constants.CustomIdentityServerScopes.WorkerTodoItem,
                    Constants.CustomIdentityServerScopes.WriteTodoItem
                }
            },
        };

        public static IEnumerable<Client> Clients(IConfiguration configuration) => new List<Client>   
        {
            // Interactive ASP.NET Core MVC client for Todo App. Also has support for client credentials grant
            new Client
            {
                ClientId = "Todo App",
                ClientSecrets = { new Secret(configuration.GetValue<string>("TodoAppSecret").Sha256()) },

                AllowedCorsOrigins = { configuration.GetValue<string>("TodoAppUrl") },
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,

                // Where to redirect to after login
                RedirectUris = 
                { 
                    $"{configuration.GetValue<string>("TodoAppUrl")}/signin-oidc", 
                    $"{configuration.GetValue<string>("TodoAppUrl")}/swagger/oauth2-redirect.html" 
                },

                // Where to redirect to after logout
                PostLogoutRedirectUris = 
                { 
                    $"{configuration.GetValue<string>("TodoAppUrl")}/signout-callback-oidc" 
                },

                FrontChannelLogoutUri = $"{configuration.GetValue<string>("TodoAppUrl")}/auth/logout",

                // Enables use of refresh tokens
                AllowOfflineAccess = true,
                AlwaysIncludeUserClaimsInIdToken = false,
                AlwaysSendClientClaims = false,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,

                    Constants.CustomIdentityServerScopes.Permission,

                    Constants.CustomIdentityServerScopes.DeleteTodoItem,
                    Constants.CustomIdentityServerScopes.ReadTodoItem,
                    Constants.CustomIdentityServerScopes.WorkerTodoItem,
                    Constants.CustomIdentityServerScopes.WriteTodoItem,
                }
            },
        };
    }
}
