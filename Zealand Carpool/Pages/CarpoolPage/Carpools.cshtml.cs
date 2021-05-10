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
    public class CarpoolsModel : PageModel
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

        public IActionResult OnGet()
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
                foreach (Carpool carpool1 in AllCarpools.Values)
                {
                    if (carpool1.PassengerSeats == carpool1.Passengerlist.Count)
                    {
                        AllCarpools.Remove(carpool1.CarpoolId);
                    }
                }
            }
            else 
            {
                AllCarpools = new Dictionary<int, Carpool>();
            }
            return Page();
        }

    }
}
