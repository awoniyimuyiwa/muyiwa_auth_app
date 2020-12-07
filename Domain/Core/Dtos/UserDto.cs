using Domain.Generic;
using System.Collections.Generic;

namespace Domain.Core.Dtos
{
    public record UserDto : BaseDto
    {
        public string UserName { get; init; }
        public string Email { get; init; }
        public bool IsSuspended { get; init; }
        public ProfileDto Profile { get; init; }
        public List<int> Roles { get; init; }
    }

    public record ProfileDto
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Gender { get; init; }
        public string DateOfBirth { get; init; }
    }
}
