using System.Threading.Tasks;
using Application;
using Application.Services.Abstracts;
using Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Permissions
{

    [Authorize(policy: Constants.PermissionNames.EditPermission)]
    public class EditModel : PageModel
    {
        readonly IPermissionService PermissionService;

        public EditModel(IPermissionService permissionService)
        {
            PermissionService = permissionService;
        }

        public Permission PermissionToEdit { get; private set; }
     
        [BindProperty]
        public CreateModel.InputModel Input { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            PermissionToEdit = await PermissionService.PermissionRepository.FindOneBy(permission => permission.Id == id);
            if (PermissionToEdit == null) { return NotFound(); }

            return Page();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(int id)
        {
            PermissionToEdit = await PermissionService.PermissionRepository.FindOneBy(permission => permission.Id == id);
            if (PermissionToEdit == null) { return NotFound(); }

            if (!ModelState.IsValid) { return Page(); }

            await PermissionService.Update(PermissionToEdit, Input.ToDto());
            TempData["Status"] = "Permission successfully updated!";

            return RedirectToPage("/admin/permissions");
        }
    }
}