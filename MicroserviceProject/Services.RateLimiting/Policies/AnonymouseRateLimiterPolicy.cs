using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;

using System.Globalization;
using System.Threading.RateLimiting;

namespace Services.RateLimiting.Policies
{
    public class AnonymouseRateLimiterPolicy : IRateLimiterPolicy<string>
    {
        private Func<OnRejectedContext, CancellationToken, ValueTask>? _onRejected;
        public static string PolicyName => "AnonymouseRateLimiterPolicy";

        public AnonymouseRateLimiterPolicy()
        {
            _onRejected = (context, cancellationToken) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter =
                        ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                }

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
                    PermitLimit = 10,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 100,
                    Window = TimeSpan.FromSeconds(1),
                    SegmentsPerWindow = 10
                });
        }
    }
}
