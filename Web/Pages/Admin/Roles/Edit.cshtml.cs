using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Application.Services.Abstracts;
using Domain.Core;
using Domain.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Roles
{

    [Authorize(policy: Constants.PermissionNames.EditRole)]
    public class EditModel : PageModel
    {
        readonly IRoleService RoleService;

        public EditModel(IRoleService roleService)
        {
            RoleService = roleService;
        }

        public Role RoleToEdit { get; private set; }
        public List<PermissionDto> PermissionDtos { get; private set; }

        [BindProperty]
        public CreateModel.InputModel Input { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            RoleToEdit = await RoleService.RoleRepository.FindOneBy(role => role.Id == id);
            if (RoleToEdit == null) { return NotFound(); }

            PermissionDtos = await RoleService.PermissionRepository.GetAllAsDto();

            return Page();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(int id)
        {
            RoleToEdit = await RoleService.RoleRepository.FindOneBy(role => role.Id == id);
            if (RoleToEdit == null) { return NotFound(); }

            if (!ModelState.IsValid) 
            {
                PermissionDtos = await RoleService.PermissionRepository.GetAllAsDto();
                return Page(); 
            }

            await RoleService.Update(RoleToEdit, Input.ToDto());
            TempData["Status"] = "Role successfully updated!";

            return RedirectToPage("/admin/roles");
        }
    }
}