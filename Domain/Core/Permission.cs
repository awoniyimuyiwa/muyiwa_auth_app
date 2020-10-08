using Domain.Core.Dtos;
using Domain.Generic;
using System;
using System.Collections.Generic;

namespace Domain.Core
{
    public class Permission : IEntity<PermissionDto>
    {
        public int Id { get; private set; }

        private string name;
        public string Name 
        { 
            get
            {
                return name;
            }
            set
            {
                name = value;
                NormalizedName = value.ToUpperInvariant();
            }
        }
        // Unique constraint will be on this property rather than on Name property so as to ensure case-insensitivity of Name Property
        public string NormalizedName { get; private set; }

        public string Description { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // M-M between Permission and Role
        // Declared just to make querying easier
        protected virtual List<PermissionRole> PermissionRoles { get; set; }

        public PermissionDto ToDto() => new PermissionDto() {
            Slug = Id.ToString(),
            Name = Name,
            Description = Description,
            CreatedAt = Formatter.Format(CreatedAt)
        };
    }
}
