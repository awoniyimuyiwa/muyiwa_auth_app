using Domain.Core;
using Domain.Core.Abstracts;
using Domain.Core.Dtos;

namespace Application.Services.Abstracts
{
    public interface IPermissionService : IService<PermissionDto, Permission>
    {
        /// <summary>
        /// 
        /// </summary>
        IPermissionRepository PermissionRepository { get; }
    }
}
