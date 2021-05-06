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
        [BindProperty]
        public string UserInput { get; set; }
        public List<User> ListOfUsers { get; set; }

        public User LoggedInUser { get; set; }
        IUser userInterface;
        public AllUserModel(IUser iuser)
        {
            userInterface = iuser;
        }
        public IActionResult OnGet(string name)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
                if (name != null)
                {
                   ListOfUsers = userInterface.SearchUsers(name); 
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
