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
    public class UserProfile : PageModel
    {
        public Comment Comment { get; set; }
        private CommentDatabase CDB;
        public User LoggedInUser { get; set; }
        public User LoggedInUser2 { get; set; }
        IUser userInterface;
        public List<Comment> UserComments { get; set; }

        public UserProfile(IUser iuser)
        {
            userInterface = iuser;
            CDB = new CommentDatabase();

        }

        
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
                UserComments = CDB.getComments(LoggedInUser.Id);
                return Page();
            }
            else
            {
                return RedirectToPage("/Index");
            }

        }

        public IActionResult OnPost()
        {
                Comment.UserID = LoggedInUser;
                Comment.UserPostID = LoggedInUser;

                CDB.AddComment(Comment);
            return RedirectToPage("UserProfile");
        }
    }
}
