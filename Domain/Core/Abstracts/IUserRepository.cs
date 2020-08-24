using Domain.Core.Dtos;
using Domain.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.Core.Abstracts
{
    public interface IUserRepository : IRepository<User, UserDto>, IUserStore<User>
    {

    }
}
