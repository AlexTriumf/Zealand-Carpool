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
        public User LoggedInUser {get;set;}
        ICarpool _carpoolInterface;
        IUser _userInterface;
        public RequestCarpoolingModel (ICarpool icarpool, IUser iuser)
        {
            _carpoolInterface = icarpool;
            _userInterface = iuser;
        }

        public IActionResult OnGet(int carpoolID)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = _userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
                Carpool = _carpoolInterface.GetCarpool(carpoolID).Result;

                return Page();
            }
            else
            {
                return RedirectToPage("/Index");
            }
        }

        public IActionResult OnPostRequestCarpool(int CarpoolId)
        {

            return Page();
        }

        public IActionResult OnPostDeleteCarpool(int CarpoolId)
        {

            return Page();
        }

    }
}
