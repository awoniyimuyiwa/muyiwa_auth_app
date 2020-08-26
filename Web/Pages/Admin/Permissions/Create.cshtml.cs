using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Application;
using Application.Services.Abstracts;
using Domain.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Permissions
{

    [Authorize(policy: Constants.PermissionNames.CreatePermission)]
    public class CreateModel : PageModel
    {
        readonly IPermissionService PermissionService;

        public CreateModel(IPermissionService permissionService)
        {
            PermissionService = permissionService;
        }

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

            public PermissionDto ToDto() => new PermissionDto
            {
                Name = Name,
                Description = Description
            };
        }

        public IActionResult OnGetAsync()
        {
            return Page();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page();  }

            await PermissionService.Create(Input.ToDto());
            TempData["Status"] = "Permission successfully created!";

            return RedirectToPage("/admin/permissions");
        }
    }
}