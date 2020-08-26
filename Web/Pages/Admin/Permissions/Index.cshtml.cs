using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Application.Services.Abstracts;
using Domain.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Permissions
{

    [Authorize(policy: Constants.PermissionNames.ViewPermission)]
    public class IndexModel : PageModel
    {
        readonly IPermissionService PermissionService;

        public IndexModel(IPermissionService permissionService)
        {
            PermissionService = permissionService;
        }

        public IList<PermissionDto> PermissionDtos { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task OnGetAsync(string search = null)
        {
            PermissionDtos = await PermissionService.PermissionRepository.FilterBy(search);
        }
    }
}