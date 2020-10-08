using Domain.Core.Abstracts.IdentityServer;
using Infrastructure.Data.Abstracts;
using Infrastructure.Data.EntityFrameworkCoreSqlServer.Repositories.IdentityServer;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data.Exceptions;

namespace Infrastructure.Data.EntityFrameworkCoreSqlServer
{
    class IdentityServerConfigurationUnitOfWork : IIdentityServerConfigurationUnitOfWork
    {
        readonly ConfigurationDbContext DbContext;
        readonly ILogger<CustomClientStore> CustomClientStoreLogger;
        readonly ILogger<CustomResourceStore> CustomResourceStoreLogger;

        // Lazy initialization
        // Note that synchronization is not needed since the lifetime of the UOW in IOC container will be scoped (i.e a unique instance per request)
        // Also the uow for a request won't be used in multiple threads at the same time
        ICustomClientStore customClientStore;
        public ICustomClientStore CustomClientStore => customClientStore ??= new CustomClientStore(DbContext, CustomClientStoreLogger);

        ICustomResourceStore customResourceStore;
        public ICustomResourceStore CustomResourceStore => customResourceStore ??= new CustomResourceStore(DbContext, CustomResourceStoreLogger);

        public IdentityServerConfigurationUnitOfWork(
            ConfigurationDbContext dbContext,
            ILogger<CustomClientStore> clientStoreLogger,
            ILogger<CustomResourceStore> resourceStoreLogger)
        {
            DbContext = dbContext;
            CustomClientStoreLogger = clientStoreLogger;
            CustomResourceStoreLogger = resourceStoreLogger;
        }

        public async Task Commit(CancellationToken cancellationToken = default)
        {
            try
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DataNotSavedException(ex);
            }
        }
    }
}
