using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using src.Entities;
using src.Models;
using src.Services;

namespace src.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Home/Login")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            Tuple<AuthenticateResponse, string> auth = await _userService.Authenticate(model);

            if (auth.Item1 == null || auth.Item2 == null)
            {
                ViewData["Message"] = "Username or password is incorrect";
                return View("Login");
            }

            CookieOptions option = new CookieOptions
            {
                // Set the secure flag, which Chrome's changes will require for SameSite none.
                // Note this will also require you to be running on HTTPS.
                Secure = true,
                // Set the cookie to HTTP only which is good practice unless you really do need
                // to access it client side in scripts.
                HttpOnly = true,
                // Add the SameSite attribute
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(15)
            };
            Response.Cookies.Append("AuthToken", auth.Item2, option);
            return Redirect("/Customer");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return Redirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
