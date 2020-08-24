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

    }
}
