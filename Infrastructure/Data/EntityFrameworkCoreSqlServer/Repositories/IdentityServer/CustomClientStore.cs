using Domain.Core.Abstracts.IdentityServer;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Models;
using Infrastructure.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntityFrameworkCoreSqlServer.Repositories.IdentityServer
{
    class CustomClientStore : ClientStore, ICustomClientStore
    {
        public CustomClientStore(
            ConfigurationDbContext context, 
            ILogger<CustomClientStore> logger) : base(context, logger) {}

        public async Task Add(Client client, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var entity = Map(client, now);
            entity.Created = now;

            await Context.Clients.AddAsync(entity, cancellationToken);
        }

        public async void Update(Client client)
        {
            var entity = await Context.Clients.Where(e => e.ClientId == client.ClientId).FirstOrDefaultAsync();
            if (entity == null) { throw new EntityNotFoundException(); }

            var now = DateTime.UtcNow;

            Context.Clients.Update(Map(client, now, entity));
        }

        public async void Delete(Client client)
        {
            var entity = await Context.Clients.Where(e => e.ClientId == client.ClientId).FirstOrDefaultAsync();
            if (entity == null) { throw new EntityNotFoundException(); }

            Context.Clients.Remove(entity);
        }

        public async Task<bool> IsEmpty(CancellationToken cancellationToken = default)
        {
            return !await Context.Clients.AnyAsync(cancellationToken);
        }

        protected IdentityServer4.EntityFramework.Entities.Client Map(
            Client model, 
            DateTime now,
            IdentityServer4.EntityFramework.Entities.Client entity = null)
        {
            entity ??= new IdentityServer4.EntityFramework.Entities.Client();

            entity.AccessTokenLifetime = model.AccessTokenLifetime;
            entity.AuthorizationCodeLifetime = model.AuthorizationCodeLifetime;
            entity.ConsentLifetime = model.ConsentLifetime;
            entity.AbsoluteRefreshTokenLifetime = model.AbsoluteRefreshTokenLifetime;
            entity.SlidingRefreshTokenLifetime = model.SlidingRefreshTokenLifetime;
            entity.RefreshTokenUsage = (int)model.RefreshTokenUsage;
            entity.UpdateAccessTokenClaimsOnRefresh = model.UpdateAccessTokenClaimsOnRefresh;
            entity.RefreshTokenExpiration = (int)model.RefreshTokenExpiration;
            entity.AccessTokenType = (int)model.AccessTokenType;
            entity.EnableLocalLogin = model.EnableLocalLogin;
            entity.IncludeJwtId = model.IncludeJwtId;
            entity.AlwaysSendClientClaims = model.AlwaysSendClientClaims;
            entity.ClientClaimsPrefix = model.ClientClaimsPrefix;
            entity.PairWiseSubjectSalt = model.PairWiseSubjectSalt;
            entity.UserSsoLifetime = model.UserSsoLifetime;
            entity.UserCodeType = model.UserCodeType;
            entity.AllowedIdentityTokenSigningAlgorithms = string.Join(", ", model.AllowedIdentityTokenSigningAlgorithms);
            entity.IdentityTokenLifetime = model.IdentityTokenLifetime;
            entity.AllowOfflineAccess = model.AllowOfflineAccess;
            entity.Enabled = model.Enabled;
            entity.ClientId = model.ClientId;
            entity.ProtocolType = model.ProtocolType;
            entity.RequireClientSecret = model.RequireClientSecret;
            entity.ClientName = model.ClientName;
            entity.Description = model.Description;
            entity.ClientUri = model.ClientUri;
            entity.LogoUri = model.LogoUri;
            entity.RequireConsent = model.RequireConsent;
            entity.DeviceCodeLifetime = model.DeviceCodeLifetime;
            entity.AllowRememberConsent = model.AllowRememberConsent;
            entity.RequirePkce = model.RequirePkce;
            entity.AllowPlainTextPkce = model.AllowPlainTextPkce;
            entity.RequireRequestObject = model.RequireRequestObject;
            entity.AllowAccessTokensViaBrowser = model.AllowAccessTokensViaBrowser;
            entity.FrontChannelLogoutUri = model.FrontChannelLogoutUri;
            entity.FrontChannelLogoutSessionRequired = model.FrontChannelLogoutSessionRequired;
            entity.BackChannelLogoutUri = model.BackChannelLogoutUri;
            entity.BackChannelLogoutSessionRequired = model.BackChannelLogoutSessionRequired;
            entity.AlwaysIncludeUserClaimsInIdToken = model.AlwaysIncludeUserClaimsInIdToken;
            entity.Updated = now;
            entity.NonEditable = false;

            entity.IdentityProviderRestrictions = model.IdentityProviderRestrictions.Select(provider => new IdentityServer4.EntityFramework.Entities.ClientIdPRestriction
            {
                Provider = provider,
                Client = entity,
                ClientId = entity.Id
            }).ToList();

            entity.Claims = model.Claims.Select(claim => new IdentityServer4.EntityFramework.Entities.ClientClaim
            {
                Type = claim.Type,
                Value = claim.Value,
                Client = entity,
                ClientId = entity.Id
            }).ToList();

            entity.AllowedCorsOrigins = model.AllowedCorsOrigins.Select(origin => new IdentityServer4.EntityFramework.Entities.ClientCorsOrigin
            {
                Origin = origin,
                Client = entity,
                ClientId = entity.Id
            }).ToList();

            entity.Properties = model.Properties.Select(keyValuePair => new IdentityServer4.EntityFramework.Entities.ClientProperty
            {
                Key = keyValuePair.Key,
                Value = keyValuePair.Value,
                Client = entity,
                ClientId = entity.Id
            }).ToList();

            entity.AllowedScopes = model.AllowedScopes.Select(scope => new IdentityServer4.EntityFramework.Entities.ClientScope
            {
                Scope = scope,
                Client = entity,
                ClientId = entity.Id
            }).ToList();

            entity.ClientSecrets = model.ClientSecrets.Select(secret => new IdentityServer4.EntityFramework.Entities.ClientSecret
            {
                Description = secret.Description,
                Value = secret.Value,
                Expiration = secret.Expiration,
                Type = secret.Type,
                Created = now,
                ClientId = entity.Id
            }).ToList();

            entity.AllowedGrantTypes = model.AllowedGrantTypes.Select(grantType => new IdentityServer4.EntityFramework.Entities.ClientGrantType
            {
                GrantType = grantType,
                Client = entity,
                ClientId = entity.Id
            }).ToList();

            entity.RedirectUris = model.RedirectUris.Select(uri => new IdentityServer4.EntityFramework.Entities.ClientRedirectUri
            {
                RedirectUri = uri,
                Client = entity,
                ClientId = entity.Id
            }).ToList();

            entity.PostLogoutRedirectUris = model.PostLogoutRedirectUris.Select(uri => new IdentityServer4.EntityFramework.Entities.ClientPostLogoutRedirectUri
            {
                PostLogoutRedirectUri = uri,
                Client = entity,
                ClientId = entity.Id
            }).ToList();

            return entity;
        }
    }
}
