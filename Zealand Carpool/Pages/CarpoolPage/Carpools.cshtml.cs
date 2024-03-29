using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;
using Zealand_Carpool.Pages.Shared;

namespace Zealand_Carpool.Pages.CarpoolPage
{
    /// <summary>
    /// The PageModel to see all carpools
    /// Made by Andreas
    /// </summary>
    public class CarpoolsModel : ProtectedPage
    {
        
        public Dictionary<int,Carpool> AllCarpools { get; set; }
        [BindProperty]
        public DateTime Date { get; set; }
        [BindProperty]
        public string Search { get; set; }


        ICarpool _carpoolInterface;

        public CarpoolsModel(ICarpool icar)
        {
            _carpoolInterface = icar;
        }

        protected override IActionResult GetRequest()
        {
            Date = DateTime.Today;
            
            AllCarpools = _carpoolInterface.GetAllCarpools(Date).Result;
            foreach (Carpool carpool1 in AllCarpools.Values)
            {
                if (carpool1.PassengerSeats == carpool1.Passengerlist.Count)
                {
                    AllCarpools.Remove(carpool1.CarpoolId);
                }
            }
            return Page();
        }

        public IActionResult OnPost() 
        {
            if (!String.IsNullOrWhiteSpace(Search))
            {
                AllCarpools = _carpoolInterface.GetAllCarpools(Date, Search.ToLower()).Result;

            }
            else 
            {
                AllCarpools = new Dictionary<int, Carpool>();
            }
            return Page();
        }

    }
}
