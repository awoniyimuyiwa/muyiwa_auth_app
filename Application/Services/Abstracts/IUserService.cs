using Domain.Core;
using Domain.Core.Abstracts;
using Domain.Core.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Abstracts
{
    public interface IUserService : IService<UserDto, User>
    {
        /// <summary>
        /// 
        /// </summary>
        IUserRepository UserRepository { get; }

        /// <summary>
        /// 
        /// </summary>
        public IRoleRepository RoleRepository { get; }

        /// <summary>
        /// 
        /// </summary>
        public IPermissionRepository PermissionRepository { get; }

        /// <summary>
        /// Set the change password attribute of a User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateChangePassword(User user, bool status = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Set the is suspended attribute of a User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateIsSuspended(
            User user, 
            bool status = true, 
            CancellationToken cancellationToken = default);
    }
}
