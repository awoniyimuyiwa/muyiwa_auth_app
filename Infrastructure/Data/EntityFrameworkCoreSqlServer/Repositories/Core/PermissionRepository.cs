using Domain.Core;
using Domain.Core.Abstracts;
using Domain.Core.Dtos;
using Domain.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntityFrameworkCoreSqlServer.Repositories.Core
{
    class PermissionRepository : IPermissionRepository
    {
        readonly DbContext DbContext;
        readonly DbSet<Permission> DbSet;

        public PermissionRepository(DbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Permissions;
        }

        public async Task<Permission> Add(
            Permission permission, CancellationToken cancellationToken = default)
        {
            permission = (await DbSet.AddAsync(permission, cancellationToken)).Entity;

            var now = DateTime.UtcNow;
            DbContext.Entry(permission).Property(permission => permission.CreatedAt).CurrentValue = now;
            DbContext.Entry(permission).Property(permission => permission.UpdatedAt).CurrentValue = now;

            return permission;
        }

        public Task<Permission> Find(int id, CancellationToken cancellationToken = default)
        {
            IQueryable<Permission> queryable = DbSet;

            queryable = queryable.Where(u => u.Id == id);

            return queryable.FirstAsync(cancellationToken);
        }

        public Task<Permission> FindOneBy(
            Expression<Func<Permission, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Permission> queryable = DbSet;

            queryable = queryable.Where(predicate);

            return queryable.FirstAsync(cancellationToken);
        }

        public void Update(Permission permission)
        {
            if (DbContext.Entry(permission).State == EntityState.Detached)
            {
                DbSet.Attach(permission);
            }

            DbContext.Entry(permission).Property(permission => permission.UpdatedAt).CurrentValue = DateTime.UtcNow;
            DbContext.Entry(permission).State = EntityState.Modified;
        }

        public void Delete(Permission permission)
        {
            if (DbContext.Entry(permission).State == EntityState.Detached)
            {
                DbSet.Attach(permission);
            }

            DbSet.Remove(permission);
        }

        public Task<long> Count(
            Expression<Func<Permission, bool>> predicate = null,
            CancellationToken cancellationToken = default)
        {
            if (predicate != null)
            {
                return DbSet.LongCountAsync(predicate, cancellationToken);
            }

            return DbSet.LongCountAsync(cancellationToken);
        }

        public Task<List<Permission>> GetAll(
            Func<IQueryable<Permission>, IOrderedQueryable<Permission>> orderBy = null,
            Expression<Func<Permission, bool>> predicate = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Permission> queryable = DbSet;
            queryable = (predicate != null) ? queryable.Where(predicate) : queryable;
            queryable = (orderBy != null) ? orderBy(queryable) : queryable;

            return queryable.ToListAsync(cancellationToken);
        }

        public Task<List<PermissionDto>> GetAllAsDto(
            Func<IQueryable<Permission>, IOrderedQueryable<Permission>> orderBy = null,
            Expression<Func<Permission, bool>> predicate = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Permission> queryable = DbSet;
            queryable = (predicate != null) ? queryable.Where(predicate) : queryable;
            queryable = (orderBy != null) ? orderBy(queryable) : queryable;

            var dtoQuery = queryable.Select(u => u.ToDto()).AsNoTracking();

            return dtoQuery.ToListAsync(cancellationToken);
        }

        public Task<PaginatedList<PermissionDto>> FilterBy(
          string search,
          int page = 1,
          int perPage = 15,
          Expression<Func<Permission, bool>> predicate = null,
          CancellationToken cancellationToken = default)
        {
            IQueryable<Permission> queryable = DbSet;

            if (predicate != null) { queryable = queryable.Where(predicate); }

            if (!string.IsNullOrEmpty(search)) { queryable = DoSearch(queryable, search); }

            var dtoQueryable = queryable.Select(u => u.ToDto()).AsNoTracking();

            return Paginator<PermissionDto>.PaginateAsync(dtoQueryable, page, perPage, cancellationToken);
        }

        public async Task<bool> IsEmpty(CancellationToken cancellationToken = default)
        {
            return !await DbSet.AnyAsync(cancellationToken);
        }

        /// <summary>
        /// Note that EF.Functions.FreeText relies on full text indexing feature in MSSQLServer
        /// which improves search performance compared to using sql like to search. 
        /// 
        /// To start using full text search, full text indexing has to be enabled in SQL server (available in Developer edition and above, not availabale in SQLExpress),
        /// if full text indexing is not already enabled you will need to run the sql server installation centre again, click on installation->New sql server stand alone installation or add features to an existing installation.
        /// When asked for installation directory mine was C:\SQL2019\Developer_ENU.
        /// Step through the wizard until feature selection and tick the check-box for full-text indexing. 
        /// After that you also need to create a full text catlogue for your DB, you can name it TodoAppFullTextCatalogue. See more here:
        /// https://docs.microsoft.com/en-us/sql/relational-databases/search/get-started-with-full-text-search?view=sql-server-ver15
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        IQueryable<Permission> DoSearch(IQueryable<Permission> queryable, string search)
        {
            return queryable.Where(permission => EF.Functions.FreeText(permission.Name, search));
        }
    }
}
