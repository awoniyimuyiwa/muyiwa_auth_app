using Domain.Core.Dtos;
using Domain.Generic;
using Microsoft.AspNetCore.Identity;

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
    }
}
