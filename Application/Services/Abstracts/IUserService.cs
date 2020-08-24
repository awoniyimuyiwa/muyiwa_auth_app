using Domain.Core;
using Domain.Core.Abstracts;
using Domain.Core.Dtos;
using System.Threading.Tasks;

namespace Application.Services.Abstracts
{
    public interface IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        IUserRepository UserRepository { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>User</returns>
        Task<User> Create(UserDto userDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userDto"></param>
        Task Update(User user, UserDto userDto);

        /// <summary>
        /// Set the change password attribute of a User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task UpdateChangePassword(User user, bool status = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        Task Delete(User user);
    }
}
