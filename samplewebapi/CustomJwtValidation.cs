using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace corewebapiwithswaggerui
{
    public class CustomJwtValidation
    {
        private readonly RequestDelegate _next;
        static StringValues jwtAccessToken;

        public CustomJwtValidation(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            CustomValidateJwt(httpContext);
            return _next(httpContext);
        }

        public static JwtReader CustomValidateJwt(HttpContext httpContext)
        {
            //if (string.IsNullOrEmpty(jwtAccessToken))
            {
                string authorization = httpContext.Request.Headers["X-MS-EndUserAuthorization"];

                // If no authorization header found, nothing to process further
                if (string.IsNullOrEmpty(authorization))
                {
                    return null;
                }

                if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    jwtAccessToken = authorization.Substring("Bearer ".Length).Trim();
                }

                // If no token found, no further work possible
                if (string.IsNullOrEmpty(jwtAccessToken))
                {
                    return null;
                }

                return new JwtReader(jwtAccessToken);
            }            
        }
    }

    public class JwtReader
    {
        static JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
        static JwtSecurityToken token = null;
        public static JwtHeader JwtHeader;

        public JwtReader(string jwtInText)
        {

            token = jwtHandler.ReadJwtToken(jwtInText);            
        }

        public JwtHeader JwtInHeader
        {
            get
            {

                //Extract the headers of the JWT
                return token.Header;
            }
        }

        public IEnumerable<Claim> JwtCliams
        {
            get
            {
                
                //Extract the headers of the JWT
                return token.Claims;
            }
        }
    }
}
