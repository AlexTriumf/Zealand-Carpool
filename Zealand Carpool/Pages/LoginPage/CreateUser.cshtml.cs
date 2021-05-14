using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Pages.LoginPage
{
    /// <summary>
    /// A PageModel to create a user
    /// Made by Andreas
    /// </summary>
    public class CreateUserModel : PageModel
    {
        [BindProperty]
        public User CreateUser { get; set; }
        [BindProperty]
        public Address Address1 { get; set; }
        [BindProperty]
        public int PostalCode { get; set; }

        public SelectList PostalCodes { get; set; }
        public User LoggedInUser { get; set; }

        IUser _userInterface;

        public CreateUserModel(IUser iuser)
        {
            _userInterface = iuser;
            
        }

        public async Task OnGetAsync()
        {

                List<Branch> postals = await _userInterface.GetAllPostalCodes();
                PostalCodes = new SelectList(postals, nameof(Branch.BranchPostalCode), nameof(Branch.BranchPostalCode));
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                List<System.Security.Claims.Claim> listofClaims = User.Claims.ToList();
                LoggedInUser = _userInterface.GetUser(Guid.Parse(listofClaims[0].Value)).Result;
            }
        }

        public IActionResult OnPost()
        {
            bool a = _userInterface.CheckUser(CreateUser).Result;
            if (!ModelState.IsValid || !_userInterface.CheckUser(CreateUser).Result)
            {
                List<Branch> postals = _userInterface.GetAllPostalCodes().Result;
                PostalCodes = new SelectList(postals, nameof(Branch.BranchPostalCode), nameof(Branch.BranchPostalCode));
                return Page();
            }
            CreateUser.AddressList = new List<Address>();
            CreateUser.AddressList.Add(Address1);
            CreateUser.AddressList[0].Postalcode = PostalCode;
            try
            {
            _userInterface.AddUser(CreateUser);
            } catch(AggregateException)
            {
                RedirectToPage("/Error");
            } 
            
            return RedirectToPage("/LoginPage/Login");
        }
    }
}
