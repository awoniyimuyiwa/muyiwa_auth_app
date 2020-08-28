using Domain.Core;
using Domain.Core.Abstracts;
using Domain.Core.Dtos;
using Infrastructure.Data.Abstracts;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    class UserService : Abstracts.IUserService
    {
        public UserService(IUnitOfWork uow)
        {
            Uow = uow;
        }

        readonly IUnitOfWork Uow;
        public IUserRepository UserRepository => Uow.UserRepository;
        public IRoleRepository RoleRepository => Uow.RoleRepository;
        public IPermissionRepository PermissionRepository => Uow.PermissionRepository;

        public async Task<User> Create(UserDto UserDto, CancellationToken cancellationToken = default)
        {
            var User = Map(UserDto);

            User = await UserRepository.Add(User);
            await Uow.Commit(cancellationToken);

            return User;
        }

        public Task Update(
            User user, 
            UserDto userDto, 
            CancellationToken cancellationToken = default)
        {
            Map(userDto, user);

            UserRepository.Update(user);

            return Uow.Commit(cancellationToken);
        }

        public Task UpdateChangePassword(
            User User, 
            bool status, 
            CancellationToken cancellationToken = default)
        {
            UserRepository.UpdateChangePassword(User, status);

            return Uow.Commit(cancellationToken);
        }

        public Task UpdateIsSuspended(
            User User,
            bool status,
            CancellationToken cancellationToken = default)
        {
            UserRepository.UpdateIsSuspended(User, status);

            return Uow.Commit(cancellationToken);
        }

        public Task Delete(User User, CancellationToken cancellationToken = default)
        {
            UserRepository.Delete(User);

            return Uow.Commit(cancellationToken);
        }

        private User Map(UserDto UserDto, User User = null)
        {
            User ??= new User();
            User.UserName = UserDto.UserName;
            // Fetch and set other relationship properties here too

            return User;
        }
    }
}
