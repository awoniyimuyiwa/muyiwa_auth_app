using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Application.Services.Abstracts;
using Domain.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Users
{

    [Authorize(policy: Constants.PermissionNames.ViewUser)]
    public class IndexModel : PageModel
    {
        readonly IUserService UserService;

        public IndexModel(IUserService userService)
        {
            UserService = userService;
        }

        public IList<UserDto> UserDtos { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task OnGetAsync(string search = null)
        {
            UserDtos = await UserService.UserRepository.FilterBy(search);
        }
    }
}