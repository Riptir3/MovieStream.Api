using Microsoft.Extensions.Caching.Distributed;
using System.Net;

namespace MovieStream.Api.RateLimiter
{
    public class CustomRateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;
        private readonly RateLimitConfiguration _config;

        public CustomRateLimitingMiddleware(RequestDelegate next, IDistributedCache cache, RateLimitConfiguration config)
        {
            _next = next;
            _cache = cache;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant();

            if(!_config.TryGetLimit(path,out int limit, out int window))
            {
                await _next(context);
                return;
            }

            string clientIdentifier;
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                clientIdentifier = token;
            }
            else
            {
                clientIdentifier = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            }

            var currentWindowStart = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / window;

            var windowCacheKey = $"rateLimit:{clientIdentifier}:{path}:{window}:{currentWindowStart}";
            string countString = await _cache.GetStringAsync(windowCacheKey);
            int requestCount = 0;

            if (!string.IsNullOrEmpty(countString))
                int.TryParse(countString, out requestCount);

            requestCount++;

            var expirationSeconds = window - (DateTimeOffset.UtcNow.ToUnixTimeSeconds() % window);

            await _cache.SetStringAsync(
            windowCacheKey,
            requestCount.ToString(),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expirationSeconds)
            });

            if (requestCount > limit)
            {
                if (context.Request.Method == "OPTIONS")
                {
                    await _next(context);
                    return;
                }

                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                context.Response.Headers["Access-Control-Expose-Headers"] = "Retry-After";

                var timeRemainingInWindow = window - (DateTimeOffset.UtcNow.ToUnixTimeSeconds() % window);
                context.Response.Headers["Retry-After"] = timeRemainingInWindow.ToString();

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync($"Too many requests! Try it {timeRemainingInWindow} seconds later.");
                return;
            }

            await _next(context);
        }
    }
}
