using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Pages.Userpage
{
    public class UserChatSignalRModel : PageModel
    {
        [BindProperty]
        public Chat chat { get; set; }
        [BindProperty]
        public User LoggedInUser { get; set; }
        public User User2 { get; set; }
        public List<ChatText> ChatTexts { get; set; }
        public Dictionary<Guid, User> AllUsers { get; set; }
        [BindProperty]
        public DateTime Date { get; set; }

        IChat _ichatter;

        IUser _userInterface;

        public UserChatSignalRModel(IUser iuser, IChat ichat)
        {
            _userInterface = iuser;
            _ichatter = ichat;
        }

        public void OnGet()
        {

            if (User.Identity.IsAuthenticated)
            {
                AllUsers = _userInterface.GetAllUsers().Result;
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = new Services.UserDatabaseAsync().GetUser(Guid.Parse(listofClaims[0].Value)).Result;
            }
            
            }
        
        public IActionResult OnPost()
        {
            return Page();

        }
    }
}
