using Domain.Core;
using Domain.Core.Abstracts;
using Domain.Core.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Abstracts
{
    public interface IRoleService : IService<RoleDto, Role>
    {
        /// <summary>
        /// 
        /// </summary>
        IRoleRepository RoleRepository { get; }

        /// <summary>
        /// 
        /// </summary>
        IPermissionRepository PermissionRepository { get; }
    }
}
