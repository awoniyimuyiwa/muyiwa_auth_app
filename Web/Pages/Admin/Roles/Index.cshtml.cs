using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Application.Services.Abstracts;
using Domain.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Roles
{

    [Authorize(policy: Constants.PermissionNames.ViewRole)]
    public class IndexModel : PageModel
    {
        readonly IRoleService RoleService;

        public IndexModel(IRoleService roleService)
        {
            RoleService = roleService;
        }

        public IList<RoleDto> RoleDtos { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task OnGetAsync(string search = null)
        {
            RoleDtos = await RoleService.RoleRepository.FilterBy(search);
        }
    }
}