using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;
using Zealand_Carpool.Pages.Shared;

namespace Zealand_Carpool.Pages.LoginPage
{
    /// <summary>
    /// A PageModel to update the user
    /// Made by Andreas
    /// </summary>
    public class UpdateUserModel : ProtectedPage
    {
        [BindProperty(SupportsGet = true)]
        public User UpdateUser { get; set; }
        [BindProperty(SupportsGet = true)]
        public Address Address1 { get; set; }
        [BindProperty]
        public int PostalCode { get; set; }

        public SelectList PostalCodes { get; set; }

        IUser _userInterface;

        public UpdateUserModel(IUser iuser)
        {
            _userInterface = iuser;
        }
        protected override IActionResult GetRequest()
        {
            List<Branch> postals = _userInterface.GetAllPostalCodes().Result;
            PostalCodes = new SelectList(postals, nameof(Branch.BranchPostalCode), nameof(Branch.BranchPostalCode));
              List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                UpdateUser = _userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
                return Page();
            
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                List<Branch> postals =  _userInterface.GetAllPostalCodes().Result;
                PostalCodes = new SelectList(postals, nameof(Branch.BranchPostalCode), nameof(Branch.BranchPostalCode));
                return Page();
            }
            UpdateUser.AddressList = new List<Address>();
            UpdateUser.AddressList.Add(Address1);
            UpdateUser.AddressList[0].Postalcode = PostalCode;
            _userInterface.UpdateUser(UpdateUser.Id,UpdateUser);

            return RedirectToPage("/Userpage/UserProfile");
        }
    }
}
