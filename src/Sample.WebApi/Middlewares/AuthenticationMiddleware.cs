using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sample.Application.Service.Interface;
using Sample.Domain.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApi.Middlewares
{
    public class AuthenticationMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly AppConfigOptions _appSettingsOption;

        public AuthenticationMiddleware(RequestDelegate next, IOptions<AppConfigOptions> appSettings)
        {
            _next = next;
            _appSettingsOption = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                SetUserContext(context, userService, token);

            await _next(context);
        }

        private void SetUserContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(_appSettingsOption.JwtSecret);
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;
                context.Items["User"] =  userService.GetUser(userId).Result;
            }
            catch(Exception ex)
            {

            }
        }
    }
}
