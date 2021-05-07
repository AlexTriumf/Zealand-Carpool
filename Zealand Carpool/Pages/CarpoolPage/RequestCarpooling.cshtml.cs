using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Pages.CarpoolPage
{
    public class RequestCarpoolingModel : PageModel
    {
        [BindProperty]
        public Carpool Carpool { get; set; }
        [BindProperty]
        public User LoggedInUser {get;set;}
        [BindProperty]
        public List<Passenger> Passengers { get; set; }
       
        ICarpool _carpoolInterface;
        IUser _userInterface;
        public RequestCarpoolingModel (ICarpool icarpool, IUser iuser)
        {
            _carpoolInterface = icarpool;
            _userInterface = iuser;
        }

        public IActionResult OnGet(int id)
        {

            if (User.Identity.IsAuthenticated)
            {
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = _userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
                Carpool = _carpoolInterface.GetCarpool(id).Result;
                Carpool.Passengerlist = _carpoolInterface.GetPassengers(Carpool).Result;
                
                Passengers = new List<Passenger>();
                
                    foreach (Passenger passenger in Carpool.Passengerlist.Values) {
                        passenger.User = _userInterface.GetUser(passenger.User.Id).Result;
                        Passengers.Add(passenger);
                    }
                
                
                return Page();
            }
            else
            {
                return RedirectToPage("/Index");
            }
        }

        public IActionResult OnPostRequestCarpool()
        {
            _carpoolInterface.AddPassenger(LoggedInUser,Carpool);
            return RedirectToPage("/CarpoolPage/RequestCarpooling", Carpool.CarpoolId);
        }

        public IActionResult OnPostDeleteCarpool()
        {
            _carpoolInterface.DeleteCarpool(Carpool.CarpoolId);
            return RedirectToPage("/CarpoolPage/Carpools");
        }

        public IActionResult OnPostAcceptPas(string id)
        {
            _carpoolInterface.UpdatePassenger(Guid.Parse(id), Carpool.CarpoolId);
            return RedirectToPage("/Userpage/UserCarpools");
        }
        public IActionResult OnPostDeletePasFromChau(string id)
        {
            User user = new User() { Id = Guid.Parse(id) };
            
            _carpoolInterface.DeletePassenger(user, Carpool);
            return RedirectToPage("/Userpage/UserCarpools");
        }

        public IActionResult OnPostDeletePas()
        {
            _carpoolInterface.DeletePassenger(LoggedInUser, Carpool);
            return RedirectToPage("/CarpoolPage/Carpools");
        }

    }
}
