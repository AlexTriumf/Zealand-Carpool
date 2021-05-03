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
        public Carpool Carpool { get; set; }

        ICarpool _carpoolInterface;

        public RequestCarpoolingModel (ICarpool icarpool)
        {
            _carpoolInterface = icarpool;
        }

        public void OnGet(int carpoolID)
        {
            Carpool = _carpoolInterface.GetCarpool(carpoolID).Result;
        }
    }
}
