using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Register
{
    
    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        public void OnGet()
        {

        }
    }
}