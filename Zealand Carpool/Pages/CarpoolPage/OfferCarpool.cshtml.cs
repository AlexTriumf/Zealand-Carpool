using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Models;
using Zealand_Carpool.Services;

namespace Zealand_Carpool.Pages.CarpoolPage
{
    public class OfferCarpoolModel : PageModel
    {
        [BindProperty]
        public Carpool Carpool { get; set; }

        private CarpoolDatabase CarpoolDatabase { get; set; }
        public void OnPost()
        {
            CarpoolDatabase.AddCarpool(Carpool);
        }

        public void OnGet()
        {
        }
    }
}
