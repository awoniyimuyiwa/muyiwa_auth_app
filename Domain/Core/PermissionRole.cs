namespace Domain.Core
{
    public class PermissionRole
    {
        public int PermissionId { get; private set; }
        public virtual Permission Permission { get; private set; }

        public int RoleId { get; private set; }
        public virtual Role Role { get; private set; }

        public void SetPermission(Permission permission)
        {
            Permission = permission;
            PermissionId = permission.Id;
        }

        public void SetRole(Role role)
        {
            Role = role;
            RoleId = role.Id;
        }
    }
}
