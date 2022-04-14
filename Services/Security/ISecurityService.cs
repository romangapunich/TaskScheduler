using System;

namespace TaskScheduler.Services.Security
{
    public interface ISecurityService
    {
        string GetSha256Hash(string input);
        Guid CreateCryptographicallySecureGuid();
    }
}
