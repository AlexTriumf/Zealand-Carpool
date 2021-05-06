using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Pages.Userpage
{
    public class AllUserModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string UserInput { get; set; }
        public List<User> ListOfUsers { get; set; }

        public User LoggedInUser { get; set; }
        IUser userInterface;
        public AllUserModel(IUser iuser)
        {
            userInterface = iuser;
        }
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
                if (!string.IsNullOrEmpty(UserInput))
                {
                   ListOfUsers = userInterface.SearchUsers(UserInput); 
                }
                
                return Page();
            }
            else
            {
                return RedirectToPage("/Index");
            }

        }

        public IActionResult OnPost()
        {

            return RedirectToPage("AllUser");
        }
    }
}
