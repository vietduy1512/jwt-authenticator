using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticator.Middlewares
{
    public class JwtDecoderMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtDecoderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["MyToken"];

            if (token.Any())
            {
                var isValidated = ValidateJwtToken(token);
                if (!isValidated)
                {
                    throw new Exception("Wrong");
                }
            }

            await _next(context);
        }

        private bool ValidateJwtToken(string token)
        {
            var secretkey = "kYp3s5v8y/B?E(H+MbQeThWmZq4t7w9z$C&F)J@NcRfUjXn2r5u8x/A%D*G-KaPdSgVkYp3s6v9y$B&E(H+MbQeThWmZq4t7w!z%C*F-J@NcRfUjXn2r5u8x/A?D(G+K";
            var tokenHandler = new JwtSecurityTokenHandler();
            //var data = tokenHandler.ReadJwtToken(token);

            TokenValidationParameters validationParameters =
                new TokenValidationParameters
                {
                    ValidateLifetime = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "My Company",
                    ValidAudience = "Sample",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey))
                };

            SecurityToken validatedToken = null;
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (SecurityTokenException e)
            {
                return false;
            }
            return true;
        }
    }
}
