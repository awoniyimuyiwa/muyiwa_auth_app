using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Application;
using Application.Services.Abstracts;
using Domain.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Roles
{

    [Authorize(policy: Constants.PermissionNames.CreateRole)]
    public class CreateModel : PageModel
    {
        readonly IRoleService RoleService;

        public CreateModel(IRoleService roleService)
        {
            RoleService = roleService;
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
            TempData["Status"] = "Role successfully created!";

            return RedirectToPage("/admin/roles");
        }
    }
}