using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Application.Services.Abstracts;
using Domain.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Web.Utils;

namespace Web.Pages.Admin.Roles
{

    [Authorize(policy: Constants.PermissionNames.CreateRole)]
    public class CreateModel : PageModel
    {
        readonly IRoleService RoleService;
        readonly IStringLocalizer<Status> StatusMessageLocalizer;

        public CreateModel(
            IRoleService roleService,
            IStringLocalizer<Status> statusMessageLocalizer)
        {
            RoleService = roleService;
            StatusMessageLocalizer = statusMessageLocalizer;
        }

        public List<PermissionDto> PermissionDtos { get; private set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Description")]
            public string Description { get; set; }

            [Required]
            [Display(Name = "Permissions")]
            public List<int> Permissions { get; set; }

            public RoleDto ToDto() => new RoleDto {
                Name = Name,
                Description = Description,
                Permissions = Permissions
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            PermissionDtos = await RoleService.PermissionRepository.GetAllAsDto();

            return Page();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) 
            {
                PermissionDtos = await RoleService.PermissionRepository.GetAllAsDto();
                return Page(); 
            }
                
            await RoleService.Create(Input.ToDto());
            TempData["Status"] = StatusMessageLocalizer["RoleCreated"];

            return RedirectToPage("/admin/roles");
        }
    }
}