using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Services;

namespace src.Controllers
{
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private IUserService _userService;

        public AuthorizationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost()]
        [Route("api/getToken")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            Tuple<AuthenticateResponse, string> auth = _userService.Authenticate(model);

            if (auth.Item1 == null || auth.Item2 == null) 
                return BadRequest(new { message = "Username or password is incorrect" });;

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
                Expires = DateTime.Now.AddMinutes(60)
            };  
            Response.Cookies.Append("AuthToken", auth.Item2, option);
            return Ok(auth.Item1);
        }

        [Authorize]
        [HttpGet]
        [Route("api/getUsers")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}