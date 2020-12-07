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

        // M-M between Role and User
        // Declared just to make querying easier
        protected virtual List<RoleUser> RoleUsers { get; set; }

        // M-M between Role and Permission
        protected virtual List<PermissionRole> PermissionRoles { get; set; }
        public IReadOnlyCollection<PermissionRole> ReadOnlyPermissionRoles => PermissionRoles;

        public Role() : base()
        {
            PermissionRoles = new List<PermissionRole>();
        }

        public void AddPermissions(params Permission[] permissions)
        {
            foreach(Permission permission in permissions)
            {
                var permissionRole = new PermissionRole();
                permissionRole.SetPermission(permission);
                permissionRole.SetRole(this);
                PermissionRoles.Add(permissionRole);
            }
        }

        public RoleDto ToDto() => new() 
        {
            Slug = Id.ToString(),
            Name = Name,
            Description = Description,
            CreatedAt = Formatter.Format(CreatedAt)
        };
    }
}
