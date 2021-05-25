using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;
using System.Linq;

namespace Zealand_Carpool.Pages.Userpage
{
    /// <summary>
    /// A PageModel to see all the users carpools and to which the user is passenger to
    /// Made by Andreas
    /// </summary>
    public class UserCarpoolsModel : Shared.ProtectedPage
    {
        
        public IEnumerable<KeyValuePair<int,Carpool>> Carpools { get; set; }
        [BindProperty]
        public User LoggedInUser { get; set; }

        
        ICarpool _carpoolInterface;
        IUser _userInterface;

        public UserCarpoolsModel(ICarpool icarpool, IUser iuser)
        {
            _carpoolInterface = icarpool;
            _userInterface = iuser;
        }

        protected override IActionResult GetRequest()
        {
            
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = _userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
                var selectedCarpools = from carpool in _carpoolInterface.GetAllCarpools(DateTime.Today).Result
                                       where carpool.Value.Passengerlist.ContainsKey(LoggedInUser.Id) ||
                                             carpool.Value.Driver.Id == LoggedInUser.Id
                                       select carpool;
                Carpools = selectedCarpools;
                return Page();
            
        }
    }
}
