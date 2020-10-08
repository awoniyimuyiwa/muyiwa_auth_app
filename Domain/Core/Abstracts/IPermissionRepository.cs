using Domain.Core.Dtos;
using Domain.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Core.Abstracts
{
    public interface IPermissionRepository : IRepository<Permission, PermissionDto>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of permissions. 
        /// If no predicate is specified, all permissions are retuned.
        /// </returns>
        Task<List<Permission>> GetAll(
           Func<IQueryable<Permission>, IOrderedQueryable<Permission>> orderBy = null,
           Expression<Func<Permission, bool>> predicate = null,
           CancellationToken cancellationToken = default);

        /// <summary>
        /// Get permissions for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<PermissionDto>> GetAllForUserAsDto(
            int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if a user has a permission
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissionName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> HasPermission(
            int userId, string permissionName, CancellationToken cancellationToken = default);

    }
}
