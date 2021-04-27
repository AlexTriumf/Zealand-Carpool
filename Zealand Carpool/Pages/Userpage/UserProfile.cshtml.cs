using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Pages.Userpage
{
    public class UserProfile : PageModel
    {
        public User LoggedInUser { get; set; }
        
        IUser userInterface;

        
        public UserProfile(IUser iuser)
        {
            userInterface = iuser;
        }

        
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
                return Page();
            }
            else
            {
                return RedirectToPage("/Index");
            }

        }
    }
}
