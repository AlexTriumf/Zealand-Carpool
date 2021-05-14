using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Pages.Userpage
{
    //Lavet af Aleksandar
    public class UserChatWindowModel : Shared.ProtectedPageRouting
    {
        [BindProperty]
        public ChatText Chattxts { get; set; }
        [BindProperty]
        public Chat Chat { get; set; }
        [BindProperty]
        public User LoggedInUser { get; set; }
        [BindProperty]
        public User User2 { get; set; }
        [BindProperty]
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

        protected override IActionResult GetRequest(string Id)
        {



            List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = _userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
                User2 = _userInterface.GetUser(Guid.Parse(Id)).Result;
                


                if (_ichatter.HasAChat(LoggedInUser.Id, User2.Id).Result)
                {
                    ChatTexts = _ichatter.GetChat(LoggedInUser.Id, User2.Id).Result;
                }
                else 
                {
                    
                _ichatter.AddChat(LoggedInUser.Id, User2.Id);
                ChatTexts = _ichatter.GetChat(LoggedInUser.Id, User2.Id).Result;
                }

                Chat = _ichatter.GetChatId(LoggedInUser.Id, User2.Id).Result;
                return Page();
        }

        public IActionResult OnPost()
        {
            List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
            LoggedInUser = _userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
            Chattxts.user = LoggedInUser;

           
            _ichatter.SendChat(Chattxts, Chat.ChatId);
            return RedirectToPage("UserChatWindow", User2.Id);
        }

    }
}
            
        
    
