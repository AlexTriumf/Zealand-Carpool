using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Models;
using Zealand_Carpool.Services;

namespace Zealand_Carpool.Pages.Userpage
{
    public class AddressModel : PageModel
    {
        
        public void OnGet()
        {

        }
        [BindProperty]
        public Address NewAdress { get; set; }

       


        public IActionResult OnPost()
        {
            

            return RedirectToPage("Index");

        }

    }
}
