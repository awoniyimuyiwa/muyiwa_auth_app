using System.Threading.Tasks;
using Application.Services.Abstracts;
using Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Web.Utils;

namespace Web.Pages.Admin.Permissions
{

    [Authorize(policy: Constants.PermissionNames.EditPermission)]
    public class EditModel : PageModel
    {
        readonly IPermissionService PermissionService;
        readonly IStringLocalizer<Status> StatusMessageLocalizer;

        public EditModel(
            IPermissionService permissionService,
            IStringLocalizer<Status> statusMessageLocalizer)
        {
            PermissionService = permissionService;
            StatusMessageLocalizer = statusMessageLocalizer;
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
            TempData["Status"] = StatusMessageLocalizer["PermissionUpdated"];

            return RedirectToPage("/admin/permissions");
        }
    }
}