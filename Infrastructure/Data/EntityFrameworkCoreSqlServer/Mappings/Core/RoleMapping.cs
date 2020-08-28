using Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityFrameworkCoreSqlServer.Mappings.Core
{
    class RoleMapping
    {
        public RoleMapping(EntityTypeBuilder<Role> entityTypeBuilder)
        {
            // Override AspNetRoles mapping in IdentityDbContext
            entityTypeBuilder.ToTable("Roles");
            entityTypeBuilder.Property(e => e.Name).IsRequired();
            entityTypeBuilder.Property(e => e.NormalizedName).IsRequired();
            entityTypeBuilder.Property(e => e.CreatedAt).IsRequired();
            entityTypeBuilder.Property(e => e.UpdatedAt).IsRequired();
            entityTypeBuilder.Ignore(e => e.ReadOnlyPermissionRoles);

            entityTypeBuilder.HasMany(typeof(RoleUser))
               .WithOne("Role")
               .HasForeignKey("RoleId")
               .IsRequired();

            entityTypeBuilder.HasMany(typeof(PermissionRole), "PermissionRoles")
               .WithOne("Role")
               .HasForeignKey("RoleId")
               .IsRequired();
        }
    }
}
