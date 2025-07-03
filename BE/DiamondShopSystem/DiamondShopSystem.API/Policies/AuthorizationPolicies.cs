using Microsoft.AspNetCore.Authorization;
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
    }
} 