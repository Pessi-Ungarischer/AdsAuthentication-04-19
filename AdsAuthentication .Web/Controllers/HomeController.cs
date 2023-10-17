using AdsAuthentication.Data;
using AdsAuthentication_.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AdsAuthentication_.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            AdsAuthenticationRepository aar = new();
            AdsViewModel indexVM = new()
            {
                Ads = aar.GetAds()
            };

            if (User.Identity.IsAuthenticated)
            {
                string userEmail = User.Identity.Name;
                indexVM.UserId = aar.GetUserByEmail(userEmail).Id;
            }
            return View(indexVM);
        }



        [Authorize]
        public IActionResult NewAd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewAd(string phoneNum, string description)
        {
            string userEmail = User.Identity.Name;
            AdsAuthenticationRepository aar = new();
            User user = aar.GetUserByEmail(userEmail);

            Ad ad = new()
            {
                ListerId = user.Id,
                PhoneNum = phoneNum,
                Date = DateTime.Now,
                Description = description
            };
            aar.NewAd(ad);
            return Redirect("/Home/Index");
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            AdsAuthenticationRepository aar = new();
            aar.Delete(id);
            return Redirect("/");
        }

        [Authorize]
        public IActionResult MyAccount()
        {
            AdsAuthenticationRepository aar = new();

            string userEmail = User.Identity.Name;
            int userId = aar.GetUserByEmail(userEmail).Id;

            List<Ad> ads = aar.GetAds();
            List<Ad> filteredAds = ads.Where(a => a.ListerId == userId).ToList();
           
            AdsViewModel indexVM = new()
            {
                Ads = filteredAds,
                UserId = userId
            };
            return View(indexVM);
        }
    }
}