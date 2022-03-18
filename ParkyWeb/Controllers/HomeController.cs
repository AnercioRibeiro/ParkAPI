using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ParkyWeb.Repository.IRepository;
using ParkyWeb.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
        private readonly IAccountRepository _acctRepo;
        private readonly ITrailRepository _trailRepo;

        public HomeController(ILogger<HomeController> logger, INationalParkRepository npRepo, ITrailRepository trailRepo, IAccountRepository acctRepo)
        {
            _logger = logger;
            _npRepo = npRepo;
            _trailRepo = trailRepo;
            _acctRepo = acctRepo;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel listOfParkAndTrails = new IndexViewModel()
            {
                NationalParkList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString("JWToken")),
                TrailList = await _trailRepo.GetAllAsync(SD.TrailAPIPath, HttpContext.Session.GetString("JWToken"))
            };
            return View(listOfParkAndTrails);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            User objUser = new User();
            return View(objUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User UserObj)
        {
            User objUser = await _acctRepo.LoginAsync(SD.AccountAPIPath+"authenticate/", UserObj);
            if (objUser.Token == null)
            {
                return View();
            }
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, objUser.Username));
            identity.AddClaim(new Claim(ClaimTypes.Role, objUser.Role));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            HttpContext.Session.SetString("JWToken", objUser.Token);
            TempData["alert"] = "Welcome, " + objUser.Username;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User UserObj)
        {
            bool result = await _acctRepo.RegisterAsync(SD.AccountAPIPath + "register/", UserObj);
            if (result == false)
            {
                return View();
            }
            TempData["alert"] = "Registration successful";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
