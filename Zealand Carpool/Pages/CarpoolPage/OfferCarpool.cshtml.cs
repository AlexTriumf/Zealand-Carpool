using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Pages.CarpoolPage
{
    public class OfferCarpoolModel : PageModel
    {
        private Carpool carpool = new Carpool();


        public void OnGet()
        {
        }
    }
}
