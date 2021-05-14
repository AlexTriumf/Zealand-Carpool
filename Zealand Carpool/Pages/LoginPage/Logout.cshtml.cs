using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Pages.Shared;

namespace Zealand_Carpool.Pages.LoginPage
{
    /// <summary>
    /// A Pagemodel to logout
    /// Made by Andreas
    /// </summary>
    public class LogoutModel : ProtectedPage
    {
        protected override IActionResult GetRequest()
        {
            return Page();
        }

        public IActionResult OnPostLogout()
        {
            
            HttpContext.SignOutAsync();
            
            return RedirectToPage("/index");
        }
    }
}
