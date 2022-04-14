using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskScheduler.Extensions
{
    public static class AuthorizationPolicyExtension
    {
        /// <summary>
        /// Add authorization policies from config
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="config">IConfiguration</param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizationPolicy(this IServiceCollection services, IConfiguration config)
        {
            var policyStrings =  config.GetSection("AppSettings").GetSection("Policy").Get<string[]>(); ;
            if (policyStrings == null || policyStrings.Length == 0) return services;
            services.AddAuthorization(options =>
            {
                foreach (var item in policyStrings)
                {
                    options.AddPolicy(item, policy => policy.RequireRole(item));
                }

            });
            return services;
        }
    }
}
