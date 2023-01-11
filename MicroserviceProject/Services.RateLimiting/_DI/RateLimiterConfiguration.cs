using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Services.RateLimiting.Policies;

namespace Services.RateLimiting.DI
{
    public static class RateLimiterConfiguration
    {
        public static IServiceCollection RegisterDefaultFixedRateLimiterPolicy(this IServiceCollection services)
        {
            services.AddRateLimiter(limiterOptions =>
            {
                limiterOptions.AddPolicy<string, DefaultFixedLimiterPolicy>(DefaultFixedLimiterPolicy.PolicyName);
            });

            return services;
        }

        public static IServiceCollection RegisterAnonymouseRateLimiterPolicy(this IServiceCollection services)
        {
            services.AddRateLimiter(limiterOptions =>
            {
                limiterOptions.AddPolicy<string, AnonymouseRateLimiterPolicy>(AnonymouseRateLimiterPolicy.PolicyName);
            });

            return services;
        }
    }
}
