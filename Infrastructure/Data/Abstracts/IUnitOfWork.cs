using Domain.Core.Abstracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.Abstracts
{
    public interface IUnitOfWork
    {
        IPermissionRepository PermissionRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }

        /// <summary>
        /// Persists changes on entities to data source
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Commit(CancellationToken cancellationToken = default);

        /// <summary>
        /// Wraps and executes the specified action within a single transaction. 
        /// The action should preferably be one in which IUnitOfWork.Commit() is called or
        /// DbContext.SaveChanges()/DbContext.SaveChangesAsync() is called in other third party libraries. 
        /// </summary>
        void Transactional(Action<IUnitOfWork> action);
    }
}
