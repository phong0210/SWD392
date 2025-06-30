using System.Collections.Concurrent;
using System.Net;

namespace DiamondShopSystem.API.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ConcurrentDictionary<string, RateLimitInfo> _rateLimitStore;
        private readonly int _maxRequests;
        private readonly TimeSpan _window;

        public RateLimitingMiddleware(RequestDelegate next, int maxRequests = 5, int windowMinutes = 15)
        {
            _next = next;
            _rateLimitStore = new ConcurrentDictionary<string, RateLimitInfo>();
            _maxRequests = maxRequests;
            _window = TimeSpan.FromMinutes(windowMinutes);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.Request.Path.Value;
            
            // Apply rate limiting to authentication endpoints
            if (IsAuthenticationEndpoint(endpoint))
            {
                var clientId = GetClientIdentifier(context);
                
                if (!IsRequestAllowed(clientId))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    
                    var response = new
                    {
                        StatusCode = 429,
                        Message = "Too many requests. Please try again later.",
                        Reason = "Rate limit exceeded",
                        IsSuccess = false,
                        RetryAfter = GetRetryAfterSeconds(clientId)
                    };
                    
                    await context.Response.WriteAsJsonAsync(response);
                    return;
                }
            }

            await _next(context);
        }

        private bool IsAuthenticationEndpoint(string? endpoint)
        {
            if (string.IsNullOrEmpty(endpoint)) return false;
            
            return endpoint.Contains("/api/auth/forgot-password", StringComparison.OrdinalIgnoreCase) ||
                   endpoint.Contains("/api/auth/login", StringComparison.OrdinalIgnoreCase) ||
                   endpoint.Contains("/api/auth/reset-password", StringComparison.OrdinalIgnoreCase);
        }

        private string GetClientIdentifier(HttpContext context)
        {
            // Use IP address as primary identifier
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            
            // For additional security, you could combine with User-Agent or other headers
            var userAgent = context.Request.Headers.UserAgent.ToString();
            
            return $"{ipAddress}:{userAgent}";
        }

        private bool IsRequestAllowed(string clientId)
        {
            var now = DateTime.UtcNow;
            
            if (_rateLimitStore.TryGetValue(clientId, out var rateLimitInfo))
            {
                // Check if window has expired
                if (now - rateLimitInfo.WindowStart > _window)
                {
                    // Reset for new window
                    rateLimitInfo.RequestCount = 1;
                    rateLimitInfo.WindowStart = now;
                    return true;
                }
                
                // Check if within limits
                if (rateLimitInfo.RequestCount < _maxRequests)
                {
                    rateLimitInfo.RequestCount++;
                    return true;
                }
                
                return false;
            }
            
            // First request for this client
            _rateLimitStore.TryAdd(clientId, new RateLimitInfo
            {
                RequestCount = 1,
                WindowStart = now
            });
            
            return true;
        }

        private int GetRetryAfterSeconds(string clientId)
        {
            if (_rateLimitStore.TryGetValue(clientId, out var rateLimitInfo))
            {
                var timeRemaining = _window - (DateTime.UtcNow - rateLimitInfo.WindowStart);
                return Math.Max(1, (int)timeRemaining.TotalSeconds);
            }
            
            return 60; // Default 1 minute
        }

        private class RateLimitInfo
        {
            public int RequestCount { get; set; }
            public DateTime WindowStart { get; set; }
        }
    }
} 