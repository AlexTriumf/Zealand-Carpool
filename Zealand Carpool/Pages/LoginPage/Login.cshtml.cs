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
    public class LoginModel : PageModel
    {
        IUser UserInterface;
        [BindProperty]
        public User user { get; set; }

        public LoginModel(IUser iUser)
        {
            UserInterface = iUser;
        }
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Task<User> taskUser = UserInterface.GetUser(user.Email, user.Password);
            
            User loggedInUser = taskUser.Result;
            
            if (loggedInUser != null && loggedInUser.Email == user.Email)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username", loggedInUser.Id.ToString()));
            
                // Handshake between server and PC identification
                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return RedirectToPage("/Carpool/Carpools");
            }
            return Page();
                


        }
    }
}
