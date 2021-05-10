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
<<<<<<< HEAD
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
            Date = DateTime.Today;
=======
        protected override IActionResult GetRequest()
        {
            
                Date = DateTime.Today;
>>>>>>> master
                
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = _userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
           
                var selectedCarpools = from carpool in _carpoolInterface.GetAllCarpools(LoggedInUser.Id).Result
                                       select carpool;
                selectedCarpools = selectedCarpools.Where(carpool => carpool.Value.Passengerlist.ContainsKey(LoggedInUser.Id) ||
                                                                     carpool.Value.Driver.Id == LoggedInUser.Id);
                Carpools = selectedCarpools;
                return Page();
<<<<<<< HEAD
            }
            else
            {
                return RedirectToPage("/Index");
            }
            
=======
                           
>>>>>>> master
        }

        public IActionResult OnPost()
        {
            if (!String.IsNullOrWhiteSpace(Search))
            {
                Carpools = _carpoolInterface.GetAllCarpools(Date, Search.ToLower()).Result;
            }
            else
            {
               Carpools = new Dictionary<int, Carpool>();
            }
            return Page();
        }
    }
}
