using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Entities;
using Backend.Services;

namespace Backend.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettingsAccessor _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettingsAccessor> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            //var token = context.Request.Cookies["AuthToken"];
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != null)
                await attachUserToContext(context, userService, token);

            await _next(context);
        }

        private async Task attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                Tuple<User, string> userValidation = await userService.ReAuthenticate(token);
                if (userValidation.Item1 == null || userValidation.Item2 == null) return;

                context.Items["User"] = userValidation.Item1;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}