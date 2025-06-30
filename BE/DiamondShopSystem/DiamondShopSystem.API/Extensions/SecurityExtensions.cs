using DiamondShopSystem.BLL.Utils;
using DiamondShopSystem.API.Middlewares;

namespace DiamondShopSystem.API.Extensions
{
    public static class SecurityExtensions
    {
        public static IServiceCollection AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure security settings
            services.Configure<SecuritySettings>(configuration.GetSection("Security"));
            
            // Add CORS with security settings
            var allowedOrigins = configuration.GetSection("Security:AllowedOrigins").Get<string[]>() ?? new string[0];
            
            services.AddCors(options =>
            {
                options.AddPolicy("SecureCorsPolicy", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSecurityMiddleware(this IApplicationBuilder app, IConfiguration configuration)
        {
            var enableRateLimiting = configuration.GetValue<bool>("Security:EnableRateLimiting", true);
            
            if (enableRateLimiting)
            {
                app.UseMiddleware<RateLimitingMiddleware>();
            }

            return app;
        }
    }
} 