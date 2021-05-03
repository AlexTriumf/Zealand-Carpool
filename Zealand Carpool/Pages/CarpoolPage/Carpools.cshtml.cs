using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Interfaces;

namespace Zealand_Carpool.Pages.CarpoolPage
{
    public class CarpoolsModel : PageModel
    {
        
        public List<Models.Carpool> AllCarpools { get; set; }
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
            _carpoolInterface.GetAllCarpools(Date);

        }

    }
}
