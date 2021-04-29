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

        [Authorize]
        public void OnGet()
        {
            List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
            userInterface.GetUser(Guid.Parse(listofClaims[0].Value));

        }
    }
}
