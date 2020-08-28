using Microsoft.AspNetCore.Identity;

namespace Domain.Core
{
    public class RoleUser: IdentityUserRole<int>
    {
        public virtual Role Role { get; private set; }
        public virtual User User { get; private set; }

        public void SetRole(Role role)
        {
            Role = role;
            RoleId = role.Id;
        }

        public void SetUser(User user)
        {
            User = user;
            UserId = user.Id;
        }
    }
}
