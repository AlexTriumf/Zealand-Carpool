using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Zealand_Carpool.Pages.Shared
{
    /// <summary>
    /// A class to check if the user is logged in, with a routing string
    /// Made by Andreas
    /// </summary>
    public abstract class ProtectedPageRouting : PageModel
    {
        public IActionResult OnGet(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            return GetRequest(id);
        }
        protected abstract IActionResult GetRequest(string id);
    }
}
