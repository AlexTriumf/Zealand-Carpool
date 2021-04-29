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
    public class UpdateUserModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public User UpdateUser { get; set; }
        [BindProperty(SupportsGet = true)]
        public Address Address1 { get; set; }

        IUser _userInterface;

        public UpdateUserModel(IUser iuser)
        {
            _userInterface = iuser;
        }
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                UpdateUser = _userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
                return Page();
            } else { return RedirectToPage("/Index"); }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            UpdateUser.AddressList = new List<Address>();
            UpdateUser.AddressList.Add(Address1);
            _userInterface.UpdateUser(UpdateUser.Id,UpdateUser);

            return RedirectToPage("/Userpage/UserProfile");
        }
    }
}
