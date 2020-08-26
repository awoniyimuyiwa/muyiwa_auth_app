using Domain.Core.Dtos;
using Domain.Generic;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Core
{
    public class Role : IdentityRole<int>, IEntity<RoleDto>
    {
        public string Description { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // M-M between Role and Permission
        protected virtual List<PermissionRole> PermissionRoles { get; set; }
        public IReadOnlyCollection<PermissionRole> ReadOnlyPermissionRoles => PermissionRoles;

        public Role() : base()
        {
            PermissionRoles = new List<PermissionRole>();
        }

        public void AddPermissions(params Permission[] permissions)
        {
            var permissionRole = new PermissionRole();
            foreach(Permission permission in permissions)
            {
                permissionRole.SetPermission(permission);
                permissionRole.SetRole(this);
                PermissionRoles.Add(permissionRole);
            }
        }

        public RoleDto ToDto() => new RoleDto() {
            Slug = Id.ToString(),
            Name = Name,
            Description = Description
        };
    }
}
