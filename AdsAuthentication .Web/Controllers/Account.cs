using AdsAuthentication.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdsAuthentication_.Web.Controllers
{
    public class Account : Controller
    {
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User user, string password)
        {

            AdsAuthenticationRepository aar = new();
            aar.SignUp(user, password);
            return Redirect("/Account/Login");
        }

        public IActionResult LogIn()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var claims = new List<Claim>
            {
                new Claim("user", email)
            };


            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role")))
                .Wait();


            AdsAuthenticationRepository aar = new();
            bool isValid = aar.Login(email, password);
            if (!isValid)
            {
                TempData["Message"] = "Invalid login";
                return Redirect("/account/login");
            }
            return Redirect("/Home/Index");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/Home/Index");
        }
    }
}
