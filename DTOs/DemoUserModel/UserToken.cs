using System;

namespace TaskScheduler.DTOs.DemoUserModel
{
    public class UserToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string AccessTokenHash { get; set; }
        public DateTime? AccessTokenExpiresDateTime { get; set; }
        public string RefreshTokenIdHashSource { get; set; }
        public string RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiresDateTime { get; set; }
    }
}
