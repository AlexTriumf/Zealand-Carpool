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
    public class OfferCarpoolModel : PageModel
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

        public async Task<IActionResult> OnGetAsync()
        {
            Carpool = new Carpool();
            
            
            if (User.Identity.IsAuthenticated)
            {
                Carpool.Date = DateTime.Today;
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                Carpool.Driver = await _userInterface.GetUser(Guid.Parse(listofClaims[0].Value));
                List<Branch> allBranches = await _carpoolInterface.GetBranches();
                Branches = new SelectList(allBranches,nameof(Branch.BranchId),nameof(Branch.BranchName));
                
                return Page();
            }
            else
            {
                return RedirectToPage("/Index");
            }
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
