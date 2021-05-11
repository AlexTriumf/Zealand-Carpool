using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Pages.LoginPage
{
    /// <summary>
    /// A PageModel to login
    /// Made by Andreas
    /// </summary>
    public class LoginModel : PageModel
    {
        IUser _userInterface;
        [BindProperty]
        public User user { get; set; }
        public bool WrongCredintials { get; set; }

        public LoginModel(IUser iUser)
        {
            _userInterface = iUser;
        }
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (user.Email is null && user.Password is null)
            {
                return Page();
            }
            Task<User> taskUser = _userInterface.GetUser(user.Email, user.Password);
            
            User loggedInUser = taskUser.Result;
            
            if (loggedInUser != null && loggedInUser.Email == user.Email)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username", loggedInUser.Id.ToString()));
            
                // Handshake between server and PC identification
                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return RedirectToPage("/Index");
            }
            WrongCredintials = true;
            return Page();
                


        }
    }
}
