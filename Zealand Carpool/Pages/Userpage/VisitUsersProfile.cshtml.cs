using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Pages.Userpage
{
    public class VisitUsersProfileModel : PageModel
    {
        /// <summary>
        /// Written by Malte
        /// </summary>
        private IComment commentInterface;
        private IUser userInterface;
        [BindProperty]
        public User LoggedInUser { get; set; }
        [BindProperty]
        public User UsersProfile { get; set; }
        [BindProperty]
        public Comment Comment { get; set; }
        public List<Comment> UserComments { get; set; }
        public VisitUsersProfileModel(IUser iuser, IComment icomment)
        {
            userInterface = iuser;
            commentInterface = icomment;
        }
        public IActionResult OnGet(Guid id)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
                UsersProfile = userInterface.GetUser(id).Result;
                UserComments = commentInterface.getComments(UsersProfile.Id);
                return Page();
            }
            else
            {
                return RedirectToPage("/Index");
            }

        }
        public IActionResult OnPost()
        {
            Comment.UserID = UsersProfile;
            Comment.UserPostID = LoggedInUser;

            commentInterface.AddComment(Comment);
            return OnGet(UsersProfile.Id);
        }
    }
}
