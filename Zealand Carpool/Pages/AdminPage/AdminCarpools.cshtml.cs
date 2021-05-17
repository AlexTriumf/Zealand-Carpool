using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Pages.AdminPage
{
    /// <summary>
    /// An Admin Page Panel
    /// Made by Andreas
    /// </summary>
    public class AdminCarpoolsModel : Shared.ProtectedPage
    {
        [BindProperty]
        public DateTime Date { get; set; }
        [BindProperty]
        public string Search { get; set; }
        public Dictionary<Guid, Statistic> AllUsersCarpools { get; set; }
        public Dictionary<Guid, Statistic> AllUsersPassengers { get; set; }
        public Dictionary<int, Carpool> AllCarpools { get; set; }
        public List<Passenger> AllPassengers { get; set; }
        ICarpool _carpoolInterface;
        IUser _userInterface;
        public AdminCarpoolsModel(ICarpool icarpool, IUser iuser)
        {
            AllUsersCarpools = new Dictionary<Guid, Statistic>();
            AllUsersPassengers = new Dictionary<Guid, Statistic>();
            _carpoolInterface = icarpool;
            _userInterface = iuser;
        }
        protected override IActionResult GetRequest()
        {
            Date = DateTime.Today;
            AllPassengers = _carpoolInterface.GetPassengersAdmin("").Result;
            AllCarpools = _carpoolInterface.GetAllCarpools(Date).Result;
            List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
            User LoggedInUser = _userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
            
            foreach (Carpool carpool in AllCarpools.Values)
            {
                if (!AllUsersCarpools.ContainsKey(carpool.Driver.Id))
                {

                    AllUsersCarpools.Add(carpool.Driver.Id, new Statistic() { Carpool = carpool, NumberOfTimes = 1 });
                } else
                {
                    AllUsersCarpools[carpool.Driver.Id].NumberOfTimes++;
                }
            }

            foreach (Passenger pas in AllPassengers)
            {
                if (!AllUsersPassengers.ContainsKey(pas.User.Id))
                {
               
                    AllUsersPassengers.Add(pas.User.Id, new Statistic() { Passenger=pas, NumberOfTimes = 1 });
                }
                else
                {
                    AllUsersPassengers[pas.User.Id].NumberOfTimes++;
                }
            }

            if (LoggedInUser.UserType != UserType.Admin)
            {
                RedirectToPage("/Index");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!String.IsNullOrWhiteSpace(Search))
            {
                AllCarpools = _carpoolInterface.GetAllCarpoolsAdmin(Date, Search.ToLower()).Result;
                AllPassengers = _carpoolInterface.GetPassengersAdmin(Search).Result;

                foreach (Carpool carpool in AllCarpools.Values)
                {
                    if (!AllUsersCarpools.ContainsKey(carpool.Driver.Id))
                    {

                        AllUsersCarpools.Add(carpool.Driver.Id, new Statistic() { Carpool = carpool, NumberOfTimes = 1 });
                    }
                    else
                    {
                        AllUsersCarpools[carpool.Driver.Id].NumberOfTimes++;
                    }
                }

                foreach (Passenger pas in AllPassengers)
                {
                    if (!AllUsersPassengers.ContainsKey(pas.User.Id))
                    {

                        AllUsersPassengers.Add(pas.User.Id, new Statistic() { Passenger = pas, NumberOfTimes = 1 });
                    }
                    else
                    {
                        AllUsersPassengers[pas.User.Id].NumberOfTimes++;
                    }
                }
            }
            else
            {
                AllCarpools = new Dictionary<int, Carpool>();
                AllPassengers = new List<Passenger>();
            }
            return Page();
        }
    }
}
