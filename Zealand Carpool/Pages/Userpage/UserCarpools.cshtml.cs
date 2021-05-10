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
    public class UserCarpoolsModel : Shared.ProtectedPage
    {
        [BindProperty]
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
                                       select carpool;
                selectedCarpools = selectedCarpools.Where(carpool => carpool.Value.Passengerlist.ContainsKey(LoggedInUser.Id) ||
                                                                     carpool.Value.Driver.Id == LoggedInUser.Id);
                Carpools = selectedCarpools;
                return Page();
            
        }
    }
}
