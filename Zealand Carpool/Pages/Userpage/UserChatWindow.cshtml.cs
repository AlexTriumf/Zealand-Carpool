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
        public User LoggedInUser { get; set; }
        public User User2 { get; set; }
        public List<ChatText> ChatTexts { get; set; }
        public Dictionary<Guid, User> AllUsers { get; set; }
        [BindProperty]
        public DateTime Date { get; set; }

        IChat _ichatter;

        IUser _userInterface;

        public UserChatWindowModel(IUser iuser, IChat ichat)
        {
            _userInterface = iuser;
            _ichatter = ichat;
        }

        public IActionResult OnGet(string Id)
        {

            if (User.Identity.IsAuthenticated)
            {
            List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList(); 
            LoggedInUser = _userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
                if (_ichatter.HasAChat(LoggedInUser.Id, Guid.Parse(Id)).Result)
                {
                    ChatTexts = _ichatter.GetChat(LoggedInUser.Id, Guid.Parse(Id)).Result;
                }
                else
                {
                    _ichatter.AddChat(LoggedInUser.Id, Guid.Parse(Id));
                    ChatTexts = _ichatter.GetChat(LoggedInUser.Id, Guid.Parse(Id)).Result;

                }
                return Page();
                
            }
            else
            {
                return RedirectToPage("/Index");
            }

        }

    }
            
        
    
}
