using Domain.Core;
using Domain.Core.Abstracts;
using Domain.Core.Dtos;
using Infrastructure.Data.Abstracts;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    class RoleService : Abstracts.IRoleService
    {
        public RoleService(IUnitOfWork uow)
        {
            Uow = uow;
        }

        readonly IUnitOfWork Uow;
        public IRoleRepository RoleRepository => Uow.RoleRepository;
        public IPermissionRepository PermissionRepository => Uow.PermissionRepository;

        public async Task<Role> Create(RoleDto RoleDto, CancellationToken cancellationToken = default)
        {
            var Role = Map(RoleDto);

            Role = await RoleRepository.Add(Role);
            await Uow.Commit(cancellationToken);

            return Role;
        }

        public Task Update(
            Role role, 
            RoleDto roleDto, 
            CancellationToken cancellationToken = default)
        {
            Map(roleDto, role);

            RoleRepository.Update(role);

            return Uow.Commit(cancellationToken);
        }

        public Task Delete(
            Role Role,
            CancellationToken cancellationToken = default)
        {
            RoleRepository.Delete(Role);

            return Uow.Commit(cancellationToken);
        }

        private Role Map(RoleDto RoleDto, Role Role = null)
        {
            Role ??= new Role();
            Role.Name = RoleDto.Name;
            Role.NormalizedName = RoleDto.Name.ToUpperInvariant();
            Role.Description = RoleDto.Description;
            // Fetch and set other relationship properties here too

            return Role;
        }
    }
}
