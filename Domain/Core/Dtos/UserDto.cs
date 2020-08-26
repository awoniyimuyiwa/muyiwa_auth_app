using Domain.Generic;
using System.Collections.Generic;

namespace Domain.Core.Dtos
{
    public class UserDto : BaseDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsSuspended { get; set; }
        public ProfileDto Profile { get; set; }
        public List<int> Roles { get; set; }
    }

    public class ProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
    }
}
