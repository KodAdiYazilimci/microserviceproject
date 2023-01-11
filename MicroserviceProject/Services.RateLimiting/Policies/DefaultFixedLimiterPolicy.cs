using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;

using System.Threading.RateLimiting;

namespace Services.RateLimiting.Policies
{
    public class DefaultFixedLimiterPolicy : IRateLimiterPolicy<string>
    {
        private Func<OnRejectedContext, CancellationToken, ValueTask>? _onRejected;
        public static string PolicyName => "DefaultFixedLimiterPolicy";

        public DefaultFixedLimiterPolicy()
        {
            _onRejected = (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return ValueTask.CompletedTask;
            };
        }

        public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => _onRejected;

        public RateLimitPartition<string> GetPartition(HttpContext httpContext)
        {
            return RateLimitPartition.GetSlidingWindowLimiter(string.Empty,
                _ => new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = 2,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0,
                    Window = TimeSpan.FromSeconds(3)
                });
        }
    }
}
