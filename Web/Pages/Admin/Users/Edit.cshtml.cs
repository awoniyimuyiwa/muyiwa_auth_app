using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Application.Services.Abstracts;
using Domain.Core;
using Domain.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Web.Utils;

namespace Web.Pages.Admin.Users
{

    [Authorize(policy: Constants.PermissionNames.EditUser)]
    public class EditModel : PageModel
    {
        readonly IUserService UserService;
        readonly IStringLocalizer<Status> StatusMessageLocalizer;

        public EditModel(
            IUserService userService,
            IStringLocalizer<Status> statusMessageLocalizer)
        {
            UserService = userService;
            StatusMessageLocalizer = statusMessageLocalizer;
        }

        public User UserToEdit { get; private set; }
        public List<RoleDto> RoleDtos { get; private set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Roles")]
            public List<int> Roles { get; set; }

            public UserDto ToDto() => new UserDto
            {
                Roles = Roles
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            UserToEdit = await UserService.UserRepository.FindOneBy(user => user.Id == id);
            if (UserToEdit == null) { return NotFound(); }

            RoleDtos = await UserService.RoleRepository.GetAllAsDto();

            return Page();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(int id)
        {
            UserToEdit = await UserService.UserRepository.FindOneBy(user => user.Id == id);
            if (UserToEdit == null) { return NotFound(); }

            if (!ModelState.IsValid)
            {
                RoleDtos = await UserService.RoleRepository.GetAllAsDto();
                return Page();
            }

            await UserService.Update(UserToEdit, Input.ToDto());
            TempData["Status"] = StatusMessageLocalizer["RoleUpdated"];

            return RedirectToPage("/admin/users");
        }
    }
}