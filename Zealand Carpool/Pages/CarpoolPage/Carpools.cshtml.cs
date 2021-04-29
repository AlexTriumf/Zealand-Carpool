using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Zealand_Carpool.Pages.CarpoolPage
{
    public class CarpoolsModel : PageModel
    {

        public List<Models.Carpool> AllCarpools { get; set; }
        public void OnGet()
        {
        }
    }
}
