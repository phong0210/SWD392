using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using DiamondShopSystem.BLL.Handlers.Auth;
using DiamondShopSystem.BLL.Handlers.User;

namespace DiamondShopSystem.BLL.Handlers
{
    public static class ValidationServiceRegistration
    {
        public static IServiceCollection AddDiamondShopValidators(this IServiceCollection services)
        {
            services.AddScoped<FluentValidation.IValidator<LoginRequestDto>, LoginRequestValidator>();
            services.AddScoped<FluentValidation.IValidator<UserCreateDto>, UserCreateValidator>();
            // Add more validators here as needed
            return services;
        }
    }
} 