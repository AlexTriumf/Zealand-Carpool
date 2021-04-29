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
    public class UserChatModel : PageModel
    {
        public User LoggedInUser {get;set;}
        public User User2 { get; set; }
        public List<ChatText> ChatTexts { get; set; }

        IChat _ichatter;
        public UserChatModel(IChat ichat)
        {
            _ichatter = ichat;
        }

        public IActionResult OnGet(string userId)
        {

            if (User.Identity.IsAuthenticated)
            {
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = new Services.UserDatabaseAsync().GetUser(Guid.Parse(listofClaims[0].Value)).Result;
                
                if (_ichatter.HasAChat(LoggedInUser.Id, Guid.Parse(userId)).Result)
                {
                    ChatTexts = _ichatter.GetChat(LoggedInUser.Id, Guid.Parse(userId)).Result;
                } else
                {
                    _ichatter.AddChat()
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
