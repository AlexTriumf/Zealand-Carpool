using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;
using Zealand_Carpool.Services;

namespace Zealand_Carpool.Pages.Userpage
{
    public class UserProfile : Shared.ProtectedPage
    {
        /// <summary>
        /// Written by Andreas and Malte
        /// </summary>
        [BindProperty]
        public Comment Comment { get; set; }


        [BindProperty]
        public User LoggedInUser { get; set; }

        public List<Comment> UserComments { get; set; }
        private IComment commentInterface;
        IUser userInterface;

        public UserProfile(IUser iuser, IComment icomment)
        {
            userInterface = iuser;
            commentInterface = icomment;
        }

        
        protected override IActionResult GetRequest()
        {
            List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
                UserComments = commentInterface.getComments(LoggedInUser.Id);
                return Page();
        }

    }
}
