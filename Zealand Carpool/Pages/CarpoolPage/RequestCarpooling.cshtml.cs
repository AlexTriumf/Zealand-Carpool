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
    /// <summary>
    /// A PageModel to see the specific carpool
    /// Made by Andreas
    /// </summary>
    public class RequestCarpoolingModel : Shared.ProtectedPageRouting
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

        protected override IActionResult GetRequest(string id)
        {
            if(id == "0")
            {
                return RedirectToPage("/CarpoolPage/Carpools");
            }
            int theID = Convert.ToInt32(id);
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = _userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
            try
            {

                Carpool = _carpoolInterface.GetCarpool(theID).Result;
            } catch (AggregateException)
            {
                return RedirectToPage("/Error");
            }
                Carpool.Passengerlist = _carpoolInterface.GetPassengers(Carpool).Result;
                
                Passengers = new List<Passenger>();
                
                    foreach (Passenger passenger in Carpool.Passengerlist.Values) {
                        passenger.User = _userInterface.GetUser(passenger.User.Id).Result;
                        Passengers.Add(passenger);
                    }
                
                
                return Page();
           
        }

        public IActionResult OnPostRequestCarpool()
        {
            try
            {
            _carpoolInterface.AddPassenger(LoggedInUser,Carpool);
            }
            catch (AggregateException ex) { return RedirectToPage("/Error"); }
            return RedirectToPage("/CarpoolPage/RequestCarpooling", Carpool.CarpoolId);
        }

        public IActionResult OnPostDeleteCarpool()
        {
            try
            {
            _carpoolInterface.DeleteCarpool(Carpool.CarpoolId);
            } catch (AggregateException ex)
            {
                return RedirectToPage("/Error");
            }
            return RedirectToPage("/CarpoolPage/Carpools");
        }

        public IActionResult OnPostAcceptPas(string pas)
        {
            try
            {
            _carpoolInterface.UpdatePassenger(Guid.Parse(pas), Carpool.CarpoolId);
            } catch (AggregateException ex)
            {
                return RedirectToPage("/Error");
            }
            return RedirectToPage("/CarpoolPage/RequestCarpooling", Carpool.CarpoolId);
        }
        public IActionResult OnPostDeletePasFromChau(string pas)
        {
            User user = new User() { Id = Guid.Parse(pas) };
            try
            {
            _carpoolInterface.DeletePassenger(user, Carpool);
            } catch (AggregateException ex) { return RedirectToPage("/Error"); }
            return RedirectToPage("/CarpoolPage/RequestCarpooling", Carpool.CarpoolId);
        }

        public IActionResult OnPostDeletePas()
        {
            try
            {
            _carpoolInterface.DeletePassenger(LoggedInUser, Carpool);
            }
            catch (AggregateException ex) { return RedirectToPage("/Error"); }
            return RedirectToPage("/CarpoolPage/Carpools");
        }

    }
}
