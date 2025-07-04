using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DiamondShopSystem.API.Policies
{
    public static class AuthorizationPolicies
    {
        public const string ManagerOnly = "ManagerOnly";
        public const string HeadOfficeAdminOnly = "HeadOfficeAdminOnly";
        public const string SaleStaffOnly = "SaleStaffOnly";
        public const string DeliveryStaffOnly = "DeliveryStaffOnly";
        public const string AdminOnly = "AdminOnly";

        // CORS Policy Constants
        public const string AllowAll = "AllowAll";
        public const string AuthEndpoints = "AuthEndpoints";
        public const string UserEndpoints = "UserEndpoints";
        public const string AdminEndpoints = "AdminEndpoints";
        public const string ProductionPolicy = "ProductionPolicy";

        public static void AddRolePolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(ManagerOnly, policy => policy.RequireRole("Manager"));
                options.AddPolicy(HeadOfficeAdminOnly, policy => policy.RequireRole("HeadOfficeAdmin"));
                options.AddPolicy(SaleStaffOnly, policy => policy.RequireRole("SaleStaff"));
                options.AddPolicy(DeliveryStaffOnly, policy => policy.RequireRole("DeliveryStaff"));
                options.AddPolicy(AdminOnly, policy => policy.RequireRole("Manager", "HeadOfficeAdmin"));
            });
        }

        public static void AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                // Development policy - allows all origins (for development only)
                options.AddPolicy(AllowAll,
                    policy => policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());

                // Auth endpoints policy - specific for authentication
                options.AddPolicy(AuthEndpoints,
                    policy => policy
                        .WithOrigins(
                            "http://localhost:3000",  // React dev server
                            "http://localhost:5173",  // Vite dev server
                            "https://localhost:3000",
                            "https://localhost:5173"
                        )
                        .AllowHeaders("Authorization", "Content-Type", "Accept")
                        .AllowMethods("POST", "OPTIONS")
                        .AllowCredentials());

                // User endpoints policy - for user management
                options.AddPolicy(UserEndpoints,
                    policy => policy
                        .WithOrigins(
                            "http://localhost:3000",
                            "http://localhost:5173",
                            "https://localhost:3000",
                            "https://localhost:5173"
                        )
                        .AllowHeaders("Authorization", "Content-Type", "Accept")
                        .AllowMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                        .AllowCredentials());

                // Admin endpoints policy - for admin operations
                options.AddPolicy(AdminEndpoints,
                    policy => policy
                        .WithOrigins(
                            "http://localhost:3000",
                            "http://localhost:3001",  // Admin panel
                            "http://localhost:5173",
                            "https://localhost:3000",
                            "https://localhost:3001",
                            "https://localhost:5173"
                        )
                        .AllowHeaders("Authorization", "Content-Type", "Accept")
                        .AllowMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                        .AllowCredentials());

                // Production policy - specific origins only
                options.AddPolicy(ProductionPolicy,
                    policy => policy
                        .WithOrigins(
                            "https://yourdomain.com",
                            "https://www.yourdomain.com",
                            "https://admin.yourdomain.com"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });
        }
    }
} 