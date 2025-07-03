using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace DiamondShopSystem.BLL.Handlers
{
    public static class ValidationServiceRegistration
    {
        public static IServiceCollection AddDiamondShopValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<UserCreateDto>, UserCreateValidator>();
            // Add more validators here as needed
            return services;
        }
    }
} 