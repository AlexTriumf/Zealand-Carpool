using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Zealand_Carpool.Pages.LoginPage
{
    /// <summary>
    /// A Pagemodel to logout
    /// Made by Andreas
    /// </summary>
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            { return Page(); }
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostLogout()
        {
            
            HttpContext.SignOutAsync();
            
            return RedirectToPage("/index");
        }
    }
}
