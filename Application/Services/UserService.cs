using Domain.Core;
using Domain.Core.Abstracts;
using Domain.Core.Dtos;
using Infrastructure.Data.Abstracts;
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

        public async Task<User> Create(UserDto UserDto)
        {
            var User = Map(UserDto);

            User = await Uow.UserRepository.Add(User);
            await Uow.Commit();

            return User;
        }

        public Task Update(User user, UserDto userDto)
        {
            Map(userDto, user);

            Uow.UserRepository.Update(user);

            return Uow.Commit();
        }

        public Task UpdateChangePassword(User User, bool status = true)
        {
            return Uow.Commit();
        }

        public Task Delete(User User)
        {
            Uow.UserRepository.Delete(User);

            return Uow.Commit();
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
