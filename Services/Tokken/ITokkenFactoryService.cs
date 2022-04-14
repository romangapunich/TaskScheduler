using TaskScheduler.DTOs.DemoUserModel;
using TaskScheduler.Services.Tokken.Models;

namespace TaskScheduler.Services.Tokken
{
    public interface ITokkenFactoryService
    {
        JwtTokensData CreateJwtTokensAsync(AspNetUsers user, string Role, int IdBoker);
        string GetRefreshTokenSerial(string refreshTokenValue);
    }
}
