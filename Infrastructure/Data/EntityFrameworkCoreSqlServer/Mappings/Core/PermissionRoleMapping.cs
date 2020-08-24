using Domain.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityFrameworkCoreSqlServer.Mappings.Core
{
    class PermissionRoleMapping
    {
        public PermissionRoleMapping(EntityTypeBuilder<PermissionRole> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(e => new { e.PermissionId, e.RoleId });
        }
    }
}
