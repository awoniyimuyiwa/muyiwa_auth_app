using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Application;
using Application.Services.Abstracts;
using Domain.Core;
using Domain.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Users
{

    [Authorize(policy: Constants.PermissionNames.EditUser)]
    public class EditModel : PageModel
    {
        readonly IUserService UserService;

        public EditModel(IUserService userService)
        {
            UserService = userService;
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
            TempData["Status"] = "User successfully updated!";

            return RedirectToPage("/admin/users");
        }
    }
}