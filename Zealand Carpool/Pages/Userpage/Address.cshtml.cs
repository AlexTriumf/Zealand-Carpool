using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Zealand_Carpool.Models;
using Zealand_Carpool.Models.Json;
using Zealand_Carpool.Services;

namespace Zealand_Carpool.Pages.Userpage
{
    public class AddressModel : PageModel
    {
        string apikey = "AIzaSyC2t8TFM7VJY_gUpk45PYxbxqqxPcasVho";
        public void OnGet()
        {

        }
        [BindProperty]
        public Address Address1 { get; set; }


        public Latitude latitude { get; set; }



        public async Task<IActionResult> OnPostAsync()
        {
            string xx = "https://maps.googleapis.com/maps/api/geocode/json?address=" + Address1.StreetName + "+" + Address1.StreetNumber + "+" + Address1.CityName + "&key=" + apikey;
            using var client = new HttpClient();
            var result = await client.GetAsync(xx);
            Latitude lat = JsonConvert.DeserializeObject<Latitude>(result.ToString());
            
            return Page();

        }

    }
}
