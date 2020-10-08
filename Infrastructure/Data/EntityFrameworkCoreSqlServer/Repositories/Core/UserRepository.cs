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
    class UserRepository : UserStore<User, Role, DbContext, int, IdentityUserClaim<int>, RoleUser, IdentityUserLogin<int>, IdentityUserToken<int>, IdentityRoleClaim<int>>, IUserRepository
    {
        readonly DbContext DbContext;
        readonly DbSet<User> DbSet;

        public UserRepository(DbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Users;

            // Override default value of AutoSaveChanges
            // since the responsibility for persisting changes (i.e calling uow.Commit()) should be left to the clients 
            AutoSaveChanges = false;
        }

        public async Task<User> Add(User user, CancellationToken cancellationToken = default)
        {
            user = (await DbSet.AddAsync(user, cancellationToken)).Entity;

            var now = DateTime.UtcNow;

            DbContext.Entry(user).Property(user => user.CreatedAt).CurrentValue = now;
            DbContext.Entry(user).Property(user => user.UpdatedAt).CurrentValue = now;

            return user;
        }

        public override async Task<IdentityResult> CreateAsync(
            User user, CancellationToken cancellationToken = default)
        {
            try
            {
                await Add(user, cancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Could not create user: {user.UserName}. {ex.Message}" });
            }
        }

        public Task<User> Find(int id, CancellationToken cancellationToken = default)
        {
            IQueryable<User> queryable = DbSet;

            queryable = queryable.Where(u => u.Id == id).Include(u => u.Profile);

            return queryable.FirstOrDefaultAsync(cancellationToken);
        }

        public Task<User> FindOneBy(
            Expression<Func<User, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            IQueryable<User> queryable = DbSet;

            queryable = queryable.Where(predicate).Include(u => u.Profile);

            return queryable.FirstOrDefaultAsync(cancellationToken);
        }

        public void Update(User user)
        {
            if (DbContext.Entry(user).State == EntityState.Detached)
            {
                DbSet.Attach(user);
            }

            DbContext.Entry(user).Property(user => user.UpdatedAt).CurrentValue = DateTime.UtcNow;
            DbContext.Entry(user).State = EntityState.Modified;
        }

        public override Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            try
            {
                Update(user);
                return Task.FromResult(IdentityResult.Success);
            }
            catch (Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = $"Could not update user: {user.UserName}. {ex.Message}" }));
            }
        }

        public void UpdateChangePassword(User user, bool status = true)
        {
            DbContext.Entry(user).Property(user => user.ChangePassword).CurrentValue = status;
            Update(user);
        }

        public void UpdateIsSuspended(User user, bool status = true)
        {
            DbContext.Entry(user).Property(user => user.IsSuspended).CurrentValue = status;
            Update(user);
        }

        public void Delete(User user)
        {
            if (DbContext.Entry(user).State == EntityState.Detached)
            {
                DbSet.Attach(user);
            }

            DbSet.Remove(user);
        }

        public override Task<IdentityResult> DeleteAsync(
            User user, CancellationToken cancellationToken = default)
        {
            try
            {
                Delete(user);
                return Task.FromResult(IdentityResult.Success);
            }
            catch (Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = $"Could not delete user: {user.UserName}. {ex.Message}" }));
            }
        }

        public Task<List<UserDto>> GetAllAsDto(
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null,
            Expression<Func<User, bool>> predicate = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<User> queryable = DbSet;
            queryable = (predicate != null) ? queryable.Where(predicate) : queryable;
            queryable = (orderBy != null) ? orderBy(queryable) : queryable;

            var dtoQuery = queryable.Select(u => u.ToDto()).AsNoTracking();

            return dtoQuery.ToListAsync(cancellationToken);
        }

        public Task<PaginatedList<UserDto>> FilterBy(
          string search,
          int page = 1,
          int perPage = 15,
          Expression<Func<User, bool>> predicate = null,
          CancellationToken cancellationToken = default)
        {
            IQueryable<User> queryable = DbSet;

            if (predicate != null) { queryable = queryable.Where(predicate); }

            if (!string.IsNullOrEmpty(search)) { queryable = DoSearch(queryable, search); }

            var dtoQueryable = queryable.Select(u => u.ToDto()).AsNoTracking();

            return Paginator<UserDto>.PaginateAsync(dtoQueryable, page, perPage, cancellationToken);
        }

        public Task<long> Count(
          Expression<Func<User, bool>> predicate = null,
          CancellationToken cancellationToken = default)
        {
            if (predicate != null)
            {
                return DbSet.LongCountAsync(predicate, cancellationToken);
            }

            return DbSet.LongCountAsync(cancellationToken);
        }

        public async Task<bool> IsEmpty(CancellationToken cancellationToken = default)
        {
            return !await DbSet.AnyAsync(cancellationToken);
        }

        IQueryable<User> DoSearch(IQueryable<User> queryable, string search)
        {
            search = search.ToUpperInvariant();
            return queryable.Where(u => EF.Functions.FreeText(u.NormalizedUserName, search) 
            || EF.Functions.FreeText(u.NormalizedEmail, search) 
            || EF.Functions.FreeText(u.PhoneNumber, search));
        }
    }
}
