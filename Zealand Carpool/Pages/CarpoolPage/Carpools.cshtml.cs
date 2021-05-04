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
        
        public List<Carpool> AllCarpools { get; set; }
        [BindProperty]
        public DateTime Date { get; set; }
        [BindProperty]
        public string Search { get; set; }


        ICarpool _carpoolInterface;

        public CarpoolsModel(ICarpool icar)
        {
            _carpoolInterface = icar;
        }

        public void OnGet()
        {
            Date = DateTime.Today;

            AllCarpools = _carpoolInterface.GetAllCarpools(Date).Result;
        }

        public IActionResult OnPost() 
        {
            if (!String.IsNullOrWhiteSpace(Search))
            {
            AllCarpools =  _carpoolInterface.GetAllCarpools(Date,Search.ToLower()).Result;

            }
            else 
            {
                AllCarpools = new List<Carpool>();
            }
            return Page();
        }

    }
}
