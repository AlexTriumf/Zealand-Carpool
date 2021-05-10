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
    public class UserChatWindowModel : PageModel
    {
        [BindProperty]
        public Chat chat { get; set; }
        [BindProperty]
        public User LoggedInUser { get; set; }
        [BindProperty]
        public User User2 { get; set; }
        [BindProperty]
        public List<ChatText> ChatTexts { get; set; }
<<<<<<< HEAD
        [BindProperty]
        public ChatText ctxt { get; set; }
        public Dictionary<Guid, User> AllUsers { get; set; }
=======
        public Dictionary<Guid, User> AllUsers { get; set; }
        [BindProperty]
>>>>>>> master
        public DateTime Date { get; set; }

        IChat _ichatter;

        IUser _userInterface;

        public UserChatWindowModel(IUser iuser, IChat ichat)
        {
            _userInterface = iuser;
            _ichatter = ichat;
        }

<<<<<<< HEAD
        public IActionResult OnGet(Guid Id)
=======
        public IActionResult OnGet(string Id)
>>>>>>> master
        {

            if (User.Identity.IsAuthenticated)
            {
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = _userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
<<<<<<< HEAD
                User2 = _userInterface.GetUser(Id).Result;
=======
                User2 = _userInterface.GetUser(Guid.Parse(Id)).Result;
>>>>>>> master

                if (_ichatter.HasAChat(LoggedInUser.Id, User2.Id).Result)
                {
                    ChatTexts = _ichatter.GetChat(LoggedInUser.Id, User2.Id).Result;
                }
                else
                {
                    _ichatter.AddChat(LoggedInUser.Id, User2.Id);
                }

                return Page();
            }

            return Page();
<<<<<<< HEAD

        }

        public IActionResult OnPost()
        {
            ctxt.user = LoggedInUser;

            _ichatter.SendChat(ctxt, chat.ChatId);
            return OnGet(LoggedInUser.Id);
=======
>>>>>>> master
        }
    }
            
        
    
}
