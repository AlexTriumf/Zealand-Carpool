using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zealand_Carpool.Pages.Shared
{
    /// <summary>
    /// A class to check if the user is logged in
    /// Made by Andreas
    /// </summary>
    public abstract class ProtectedPage : PageModel
    {
        
        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            return GetRequest();
        }
        protected abstract IActionResult GetRequest();

    }


}
