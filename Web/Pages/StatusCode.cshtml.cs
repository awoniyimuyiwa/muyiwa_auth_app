using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    [AllowAnonymous]
    public class StatusCodeModel : PageModel
    {
        public StatusCodeModel()
        {

        }

        public int Code { get; private set; }

        public string Header
        {
            get
            {
                return Code switch 
                {
                    StatusCodes.Status404NotFound => "Not Found",
                    StatusCodes.Status403Forbidden => "Forbidden",
                    _ => "Server Error"
                };
            }
        }

        public string Description
        {
            get
            {
                return Code switch
                {
                    StatusCodes.Status404NotFound => "The resource you requested either doesn't exist or has been renamed.",
                    StatusCodes.Status403Forbidden => "You are not permitted to access the requested resource. Please contact the site administrator for more details.",
                    _ => "Something went wrong. We're working on it and we'll get it fixed as soon as we can."
                };
            }
        }

        public void OnGet(int code = 500)
        {
            Code = code;
        }
    }
}