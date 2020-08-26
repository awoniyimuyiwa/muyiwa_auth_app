using Domain.Core;
using Domain.Core.Abstracts;
using Domain.Core.Dtos;
using Infrastructure.Data.Abstracts;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    class PermissionService : Abstracts.IPermissionService
    {
        public PermissionService(IUnitOfWork uow)
        {
            Uow = uow;
        }

        readonly IUnitOfWork Uow;
        public IPermissionRepository PermissionRepository => Uow.PermissionRepository;

        public async Task<Permission> Create(PermissionDto PermissionDto, CancellationToken cancellationToken = default)
        {
            var Permission = Map(PermissionDto);

            Permission = await PermissionRepository.Add(Permission);
            await Uow.Commit(cancellationToken);

            return Permission;
        }

        public Task Update(
            Permission permission, 
            PermissionDto permissionDto, 
            CancellationToken cancellationToken = default)
        {
            Map(permissionDto, permission);

            PermissionRepository.Update(permission);

            return Uow.Commit(cancellationToken);
        }

        public Task Delete(
            Permission Permission,
            CancellationToken cancellationToken = default)
        {
            PermissionRepository.Delete(Permission);

            return Uow.Commit(cancellationToken);
        }

        private Permission Map(PermissionDto PermissionDto, Permission Permission = null)
        {
            Permission ??= new Permission();
            Permission.Name = PermissionDto.Name;
            Permission.Description = PermissionDto.Description;
            // Fetch and set other relationship properties here too

            return Permission;
        }
    }
}
