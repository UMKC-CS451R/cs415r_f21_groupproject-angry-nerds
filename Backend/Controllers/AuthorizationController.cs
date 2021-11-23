using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Entities;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers
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
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            AuthenticateResponse auth = await _userService.Authenticate(model);

            if (auth == null ) 
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(auth);
        }

        [Authorize]
        [HttpGet()]
        [Route("api/refreshToken")]
        public IActionResult ReAuthenticate()
        {
            // only error is 500, which is returned automatically
            var user = (User)HttpContext.Items["User"];
            var myResponse = _userService.ReAuthenticate(user);
            if (myResponse == null) 
            { 
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); 
            }
            return Ok(myResponse);
        }
    }
}