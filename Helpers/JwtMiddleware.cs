using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using src.Entities;
using src.Services;

namespace src.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Cookies["AuthToken"];

            if (token != null)
                attachUserToContext(context, userService, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                Tuple<User, string> userValidation = userService.ReAuthenticate(token);
                if (userValidation.Item1 == null || userValidation.Item2 == null) return;

                context.Items["User"] = userValidation.Item1;

                // update authentication cookie
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
                context.Response.Cookies.Append("AuthToken", userValidation.Item2, option);
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}