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
    class CustomResourceStore : ResourceStore, ICustomResourceStore
    {
        public CustomResourceStore(
            ConfigurationDbContext context, 
            ILogger<CustomResourceStore> logger) : base(context, logger) {}

        public async Task Add(ApiScope apiScope, CancellationToken cancellationToken = default)
        {
            var entity = Map(apiScope);

            await Context.ApiScopes.AddAsync(entity, cancellationToken);
        }

        public async Task Add(ApiResource apiResource, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var entity = Map(apiResource, now);
            entity.Created = now;

            await Context.ApiResources.AddAsync(entity, cancellationToken);
        }

        public async Task Add(IdentityResource identityResource, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var entity = Map(identityResource, now);
            entity.Created = now;

            await Context.IdentityResources.AddAsync(entity, cancellationToken);
        }


        public async void Update(ApiScope apiScope)
        {
            var entity = await Context.ApiScopes.Where(e => e.Name == apiScope.Name).FirstOrDefaultAsync();
            if (entity == null) { throw new EntityNotFoundException(); }

            Context.ApiScopes.Update(Map(apiScope, entity));
        }

        public async void Update(ApiResource apiResource)
        {
            var entity = await Context.ApiResources.Where(e => e.Name == apiResource.Name).FirstOrDefaultAsync();
            if (entity == null) { throw new EntityNotFoundException(); }

            var now = DateTime.UtcNow;

            Context.ApiResources.Update(Map(apiResource, now, entity));
        }

        public async void Update(IdentityResource identityResource)
        {
            var entity = await Context.IdentityResources.Where(e => e.Name == identityResource.Name).FirstOrDefaultAsync();
            if (entity == null) { throw new EntityNotFoundException(); }

            var now = DateTime.UtcNow;

            Context.IdentityResources.Update(Map(identityResource, now, entity));
        }


        public async void Delete(ApiScope apiScope)
        {
            var entity = await Context.ApiScopes.Where(e => e.Name == apiScope.Name).FirstOrDefaultAsync();
            if (entity == null) { throw new EntityNotFoundException(); }

            Context.ApiScopes.Remove(entity);
        }

        public async void Delete(ApiResource apiResource)
        {
            var entity = await Context.ApiResources.Where(e => e.Name == apiResource.Name).FirstOrDefaultAsync();
            if (entity == null) { throw new EntityNotFoundException(); }

            Context.ApiResources.Remove(entity);
        }

        public async void Delete(IdentityResource identityResource)
        {
            var entity = await Context.IdentityResources.Where(e => e.Name == identityResource.Name).FirstOrDefaultAsync();
            if (entity == null) { throw new EntityNotFoundException(); }

            Context.IdentityResources.Remove(entity);
        }

        public async Task<bool> IsApiScopesEmpty(CancellationToken cancellationToken = default)
        {
            return !await Context.ApiScopes.AnyAsync(cancellationToken);
        }

        public async Task<bool> IsApiResourcesEmpty(CancellationToken cancellationToken = default)
        {
            return !await Context.ApiResources.AnyAsync(cancellationToken);
        }

        public async Task<bool> IsIdentityResourcesEmpty(CancellationToken cancellationToken = default)
        {
            return !await Context.IdentityResources.AnyAsync(cancellationToken);
        }

        protected IdentityServer4.EntityFramework.Entities.ApiScope Map(
          ApiScope model,
          IdentityServer4.EntityFramework.Entities.ApiScope entity = null)
        {
            entity ??= new IdentityServer4.EntityFramework.Entities.ApiScope();

            entity.Enabled = model.Enabled;
            entity.Name = model.Name;
            entity.DisplayName = model.DisplayName;
            entity.Description = model.Description;
            entity.Required = model.Required;
            entity.Emphasize = model.Emphasize;
            entity.ShowInDiscoveryDocument = model.ShowInDiscoveryDocument;

            entity.UserClaims = model.UserClaims.Select(type => new IdentityServer4.EntityFramework.Entities.ApiScopeClaim
            {
                Type = type,
                ScopeId = entity.Id,
                Scope = entity,
            }).ToList();

            entity.Properties = model.Properties.Select(keyValuePair => new IdentityServer4.EntityFramework.Entities.ApiScopeProperty
            {
                Key = keyValuePair.Key,
                Value = keyValuePair.Value,
                ScopeId = entity.Id,
                Scope = entity,
            }).ToList();

            return entity;
        }

        protected IdentityServer4.EntityFramework.Entities.ApiResource Map(
           ApiResource model,
           DateTime now,
           IdentityServer4.EntityFramework.Entities.ApiResource entity = null)
        {
            entity ??= new IdentityServer4.EntityFramework.Entities.ApiResource();

            entity.Enabled = model.Enabled;
            entity.Name = model.Name;
            entity.DisplayName = model.DisplayName;
            entity.Description = model.Description;
            entity.AllowedAccessTokenSigningAlgorithms = string.Join(", ", model.AllowedAccessTokenSigningAlgorithms);
            entity.ShowInDiscoveryDocument = model.ShowInDiscoveryDocument;
            entity.Updated = now;
            entity.NonEditable = false;

            entity.Secrets = model.ApiSecrets.Select(apiSecret => new IdentityServer4.EntityFramework.Entities.ApiResourceSecret
            {
                Description = apiSecret.Description,
                Value = apiSecret.Value,
                Expiration = apiSecret.Expiration,
                Type = apiSecret.Type,
                Created = now,
                ApiResourceId = entity.Id,
                ApiResource = entity
            }).ToList();

            entity.Scopes = model.Scopes.Select(scope => new IdentityServer4.EntityFramework.Entities.ApiResourceScope
            {
                Scope = scope,
                ApiResourceId = entity.Id,
                ApiResource = entity,
            }).ToList();

            entity.UserClaims = model.UserClaims.Select(type => new IdentityServer4.EntityFramework.Entities.ApiResourceClaim
            {
                Type = type,
                ApiResourceId = entity.Id,
                ApiResource = entity,
            }).ToList();

            entity.Properties = model.Properties.Select(keyValuePair => new IdentityServer4.EntityFramework.Entities.ApiResourceProperty
            {
                Key = keyValuePair.Key,
                Value = keyValuePair.Value,
                ApiResourceId = entity.Id,
                ApiResource = entity,
            }).ToList();

            return entity;
        }

        protected IdentityServer4.EntityFramework.Entities.IdentityResource Map(
          IdentityResource model,
          DateTime now,
          IdentityServer4.EntityFramework.Entities.IdentityResource entity = null)
        {
            entity ??= new IdentityServer4.EntityFramework.Entities.IdentityResource();

            entity.Enabled = model.Enabled;
            entity.Name = model.Name;
            entity.DisplayName = model.DisplayName;
            entity.Description = model.Description;
            entity.Required = model.Required;
            entity.Emphasize = model.Emphasize;
            entity.ShowInDiscoveryDocument = model.ShowInDiscoveryDocument;
            entity.Updated = now;
            entity.NonEditable = false;

            entity.UserClaims = model.UserClaims.Select(type => new IdentityServer4.EntityFramework.Entities.IdentityResourceClaim
            {
                Type = type,
                IdentityResourceId = entity.Id,
                IdentityResource = entity,
            }).ToList();

            entity.Properties = model.Properties.Select(keyValuePair => new IdentityServer4.EntityFramework.Entities.IdentityResourceProperty
            {
                Key = keyValuePair.Key,
                Value = keyValuePair.Value,
                IdentityResourceId = entity.Id,
                IdentityResource = entity,
            }).ToList();

            return entity;
        }
    }
}
