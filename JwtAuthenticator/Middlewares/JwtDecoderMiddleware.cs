using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
                var handler = new JwtSecurityTokenHandler();
                var data = handler.ReadJwtToken(token);
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
