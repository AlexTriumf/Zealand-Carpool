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

        public bool HasSent { get; set; }
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
                Dictionary<Guid,Passenger> passengers = _carpoolInterface.GetPassengers(Carpool).Result;
                if (passengers.ContainsKey(LoggedInUser.Id)) {
                    HasSent = true;
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

            return RedirectToPage("Index");
        }

    }
}
