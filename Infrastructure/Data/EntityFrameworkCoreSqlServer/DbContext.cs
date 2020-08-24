using Domain.Core;
using Infrastructure.Data.EntityFrameworkCoreSqlServer.Mappings.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.EntityFrameworkCoreSqlServer
{
    internal class DbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionRole> PermissionRoles { get; set; }
        
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
            // Increase timeout, seems full text search with EF.Functions.FreeText() is timing out on first search query
            Database.SetCommandTimeout(9000);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new PermissionMapping(modelBuilder.Entity<Permission>());
            new PermissionRoleMapping(modelBuilder.Entity<PermissionRole>());
            new RoleMapping(modelBuilder.Entity<Role>());
            new UserMapping(modelBuilder.Entity<User>());
        }
    }
}
