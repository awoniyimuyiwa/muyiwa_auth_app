using Domain.Core;
using Domain.Core.Abstracts;
using Domain.Core.Dtos;
using Domain.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntityFrameworkCoreSqlServer.Repositories.Core
{
    class RoleRepository : RoleStore<Role, DbContext, int, RoleUser, IdentityRoleClaim<int>>, IRoleRepository
    {
        readonly DbContext DbContext;
        readonly DbSet<Role> DbSet;

        public RoleRepository(DbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Roles;

            // Override default value of AutoSaveChanges
            // since the responsibility for persisting changes (i.e calling uow.Commit()) should be left to the clients 
            AutoSaveChanges = false;
        }

        public async Task<Role> Add(Role role, CancellationToken cancellationToken = default)
        {
            role = (await DbSet.AddAsync(role, cancellationToken)).Entity;

            var now = DateTime.UtcNow;
            DbContext.Entry(role).Property(role => role.CreatedAt).CurrentValue = now;
            DbContext.Entry(role).Property(role => role.UpdatedAt).CurrentValue = now;

            return role;
        }

        public override async Task<IdentityResult> CreateAsync(
            Role role, CancellationToken cancellationToken)
        {
            try
            {
                await Add(role, cancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Could not insert role {role.Name}. {ex.Message}" });
            }
        }

        public Task<Role> Find(int id, CancellationToken cancellationToken = default)
        {
            IQueryable<Role> queryable = DbSet;

            queryable = queryable.Where(u => u.Id == id);

            return queryable.FirstAsync(cancellationToken);
        }

        public Task<Role> FindOneBy(
            Expression<Func<Role, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Role> queryable = DbSet;

            queryable = queryable.Where(predicate);

            return queryable.FirstAsync(cancellationToken);
        }

        public void Update(Role role)
        {
            if (DbContext.Entry(role).State == EntityState.Detached)
            {
                DbSet.Attach(role);
            }

            DbContext.Entry(role).Property(role => role.UpdatedAt).CurrentValue = DateTime.UtcNow;
            DbContext.Entry(role).State = EntityState.Modified;
        }

        public override Task<IdentityResult> UpdateAsync(
            Role role, CancellationToken cancellationToken)
        {
            try
            {
                Update(role);
                return Task.FromResult(IdentityResult.Success);
            }
            catch (Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = $"Could not update role: {role.Name}. {ex.Message}" }));
            }
        }

        public void Delete(Role role)
        {
            if (DbContext.Entry(role).State == EntityState.Detached)
            {
                DbSet.Attach(role);
            }

            DbSet.Remove(role);
        }

        public override Task<IdentityResult> DeleteAsync(
            Role role, CancellationToken cancellationToken)
        {
            try
            {
                Delete(role);
                return Task.FromResult(IdentityResult.Success);
            }
            catch (Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = $"Could not delete role: {role.Name}. {ex.Message}" }));
            }
        }

        public Task<long> Count(
            Expression<Func<Role, bool>> predicate = null,
            CancellationToken cancellationToken = default)
        {
            if (predicate != null)
            {
                return DbSet.LongCountAsync(predicate, cancellationToken);
            }

            return DbSet.LongCountAsync(cancellationToken);
        }

        public Task<List<RoleDto>> GetAllAsDto(
            Func<IQueryable<Role>, IOrderedQueryable<Role>> orderBy = null,
            Expression<Func<Role, bool>> predicate = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Role> queryable = DbSet;
            queryable = (predicate != null) ? queryable.Where(predicate) : queryable;
            queryable = (orderBy != null) ? orderBy(queryable) : queryable;

            var dtoQuery = queryable.Select(u => u.ToDto()).AsNoTracking();

            return dtoQuery.ToListAsync(cancellationToken);
        }

        public Task<PaginatedList<RoleDto>> FilterBy(
          string search,
          int page = 1,
          int perPage = 15,
          Expression<Func<Role, bool>> predicate = null,
          CancellationToken cancellationToken = default)
        {
            IQueryable<Role> queryable = DbSet;

            if (predicate != null) { queryable = queryable.Where(predicate); }

            if (!string.IsNullOrEmpty(search)) { queryable = DoSearch(queryable, search); }

            var dtoQueryable = queryable.Select(u => u.ToDto()).AsNoTracking();

            return Paginator<RoleDto>.PaginateAsync(dtoQueryable, page, perPage, cancellationToken);
        }

        public async Task<bool> IsEmpty(CancellationToken cancellationToken = default)
        {
            return !await DbSet.AnyAsync(cancellationToken);
        }

        IQueryable<Role> DoSearch(IQueryable<Role> queryable, string search)
        {
            return queryable.Where(r => EF.Functions.FreeText(r.NormalizedName, search.ToUpperInvariant()));
        }
    }
}
