using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;
using Zealand_Carpool.Services;

namespace Zealand_Carpool.Pages.CarpoolPage
{
    public class OfferCarpoolModel : Shared.ProtectedPage
    {
        [BindProperty(SupportsGet = true)]
        public Carpool Carpool { get; set; }
        public SelectList Branches { get; set; }
       [BindProperty]
       public int BranchId { get; set; }

        ICarpool _carpoolInterface;
        IUser _userInterface;
        public OfferCarpoolModel(ICarpool icarpool, IUser iuser)
        {
            _userInterface = iuser;
            _carpoolInterface = icarpool;
        }

        protected override IActionResult GetRequest()
        {
            Carpool = new Carpool();
                Carpool.Date = DateTime.Today;
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                Carpool.Driver = _userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
                List<Branch> allBranches = _carpoolInterface.GetBranches().Result;
                Branches = new SelectList(allBranches,nameof(Branch.BranchId),nameof(Branch.BranchName));
                
                return Page();
            
           
        }
        public IActionResult OnPost()
        {
            if (Carpool.PassengerSeats<=0)
            {
                return RedirectToPage("/CarpoolPage/OfferCarpool");
            }
            Carpool.Branch = new Branch();
            Carpool.Branch.BranchId = BranchId;
            _carpoolInterface.AddCarpool(Carpool);
            
            return RedirectToPage("/UserPage/UserCarpools");
        }
    }
}
