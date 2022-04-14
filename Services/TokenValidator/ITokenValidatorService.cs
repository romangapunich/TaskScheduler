using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TaskScheduler.Services.TokenValidator
{
    public interface ITokenValidatorService
    {
        Task ValidateAsync(TokenValidatedContext context);
    }
}
