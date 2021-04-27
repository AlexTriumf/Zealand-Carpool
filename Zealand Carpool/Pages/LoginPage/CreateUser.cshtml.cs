using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Pages.LoginPage
{
    public class CreateUserModel : PageModel
    {
        [BindProperty]
        public User CreatedUser { get; set; }
        [BindProperty]
        public Address Address1 { get; set; }
        [BindProperty]
        public Address Address2 { get; set; }

        public User LoggedInUser { get; set; }

        IUser userInterface;

        public CreateUserModel(IUser iuser)
        {
            userInterface = iuser;
        }

        public void OnGet()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            userInterface.AddUser(CreatedUser);
            LoggedInUser.AddressList.Add(Address1);
            if (Address2 != null)
            {
                LoggedInUser.AddressList.Add(Address2);
            }
            return RedirectToPage("/LoginPage/Login");
        }
    }
}
