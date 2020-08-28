using Domain.Core.Dtos;
using Domain.Generic;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Core.Abstracts
{
    public interface IUserRepository : IRepository<User, UserDto>, IUserStore<User>
    {
        /// <summary>
        /// Update a user's ChangePassword attribute
        /// </summary>
        /// <param name="user"></param>
        /// <param name="status"></param>
        public void UpdateChangePassword(User user, bool status = true);

        /// <summary>
        /// Update a user's IsSuspended attribute
        /// </summary>
        /// <param name="user"></param>
        /// <param name="status"></param>
        public void UpdateIsSuspended(User user, bool status = true);

        /// <summary>
        /// Check if a user has permission
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissionName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> HasPermission(int userId, string permissionName, CancellationToken cancellationToken = default);
    }
}
