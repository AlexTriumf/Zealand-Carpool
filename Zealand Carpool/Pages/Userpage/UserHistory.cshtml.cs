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
    /// <summary>
    /// A PageModel to see all the carpools the user has been in or made
    /// Made by Andreas
    /// </summary>
    public class UserHistoryModel : Shared.ProtectedPage
    {
        [BindProperty]
        public IEnumerable<KeyValuePair<int, Carpool>> Carpools { get; set; }
        [BindProperty]
        public User LoggedInUser { get; set; }
        [BindProperty]
        public DateTime Date { get; set; }
        [BindProperty]
        public string Search { get; set; }

        ICarpool _carpoolInterface;
        IUser _userInterface;

        public UserHistoryModel(ICarpool icarpool, IUser iuser)
        {
            _carpoolInterface = icarpool;
            _userInterface = iuser;
        }
        protected override IActionResult GetRequest()
        {
            
                Date = DateTime.Today;
                
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = _userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
           
                var selectedCarpools = from carpool in _carpoolInterface.GetAllCarpools(LoggedInUser.Id).Result
                                       where carpool.Value.Passengerlist.ContainsKey(LoggedInUser.Id) ||
                                             carpool.Value.Driver.Id == LoggedInUser.Id
                                        select carpool;
               
                Carpools = selectedCarpools;
                return Page();
                           
        }

        public IActionResult OnPost()
        {
            if (!String.IsNullOrWhiteSpace(Search))
            {
                var selectedCarpools = from carpool in _carpoolInterface.GetAllCarpools(Date, Search.ToLower()).Result
                                       where carpool.Value.Passengerlist.ContainsKey(LoggedInUser.Id) ||
                                             carpool.Value.Driver.Id == LoggedInUser.Id
                                       select carpool;
                Carpools = selectedCarpools;
            }
            else
            {
               Carpools = new Dictionary<int, Carpool>();
            }
            return Page();
        }
    }
}
