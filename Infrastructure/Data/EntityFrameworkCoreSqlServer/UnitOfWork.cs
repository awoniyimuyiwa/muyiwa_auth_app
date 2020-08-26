using Domain.Core.Abstracts;
using Infrastructure.Data.Abstracts;
using Infrastructure.Data.EntityFrameworkCoreSqlServer.Repositories.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure.Data.EntityFrameworkCoreSqlServer
{
    class UnitOfWork : IUnitOfWork
    {
        readonly DbContext DbContext;

        // Lazy initialization
        // Note that synchronization is not needed since the lifetime of the UOW in IOC container will be scoped (i.e a unique instance per request)
        // Also the uow for a request won't be used in multiple threads at the same time
        IPermissionRepository permissionRepository;
        public IPermissionRepository PermissionRepository => permissionRepository ??= new PermissionRepository(DbContext);

        IRoleRepository roleRepository;
        public IRoleRepository RoleRepository => roleRepository ??= new RoleRepository(DbContext);

        IUserRepository userRepository;
        public IUserRepository UserRepository => userRepository ??= new UserRepository(DbContext);

        public UnitOfWork(DbContext dbContext)
        {
            DbContext = dbContext;
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

        public void Transactional(Action<IUnitOfWork> action)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                // Timeout = new TimeSpan(0, 0, 0, 10),
            };

            using var scope = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                transactionOptions,
                TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                action.Invoke(this);
                scope.Complete();
            }
            catch (Exception ex)
            {
                scope.Dispose();
                throw new DataNotSavedException(ex);
            }
        }
    }
}
