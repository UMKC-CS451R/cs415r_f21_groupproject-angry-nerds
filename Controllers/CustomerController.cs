using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Entities;
using src.Models;
using src.Services;

namespace src.Controllers
{
    public class CustomerController : Controller
    {

        private readonly ILogger<CustomerController> _logger;
        private IUserService _userService;

        public CustomerController(ILogger<CustomerController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [Authorize]
        public IActionResult Index()
        {
            User user = (User)HttpContext.Items["User"];
            ViewBag.User = new src.Models.API.UserInfo(user);
            return View();
        }
    }
}
