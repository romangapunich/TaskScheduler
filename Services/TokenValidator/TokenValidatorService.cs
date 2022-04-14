using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TaskScheduler.Services.TokenValidator
{
    public class TokenValidatorService : ITokenValidatorService
    {
        
        

        public TokenValidatorService()
        {
          
              }

        public async Task ValidateAsync(TokenValidatedContext context)
        {
            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
            {
                context.Fail("This is not our issued token. It has no claims.");
                return;
            }

            var serialNumberClaim = claimsIdentity.FindFirst(ClaimTypes.SerialNumber);
            if (serialNumberClaim == null)
            {
                context.Fail("This is not our issued token. It has no serial.");
                return;
            }

            var userId = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Fail("This is not our issued token. It has no user-id.");
                return;
            }

            var role = claimsIdentity.FindFirst(ClaimTypes.Role).Value;
            if (string.IsNullOrEmpty(role))
            {
                context.Fail("This is not our issued token. It has no Role.");
                return;
            }

        }
    }
}
