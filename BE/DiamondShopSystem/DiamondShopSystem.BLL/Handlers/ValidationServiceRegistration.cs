using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using DiamondShopSystem.BLL.Handlers.Auth.DTOs;
using DiamondShopSystem.BLL.Handlers.Auth.Validators;
using DiamondShopSystem.BLL.Handlers.User.DTOs;
using DiamondShopSystem.BLL.Handlers.User.Commands.Get;
using DiamondShopSystem.BLL.Handlers.User.Validators;
using DiamondShopSystem.BLL.Handlers.Product.Validators;
using DiamondShopSystem.BLL.Handlers.Warranty.DTOs;

namespace DiamondShopSystem.BLL.Handlers
{
    public static class ValidationServiceRegistration
    {
        public static IServiceCollection AddDiamondShopValidators(this IServiceCollection services)
        {
            services.AddScoped<FluentValidation.IValidator<LoginRequestDto>, LoginRequestValidator>();
            services.AddScoped<FluentValidation.IValidator<UserCreateDto>, UserCreateValidator>();
            services.AddScoped<FluentValidation.IValidator<UserGetCommand>, UserGetValidator>();
            services.AddScoped<FluentValidation.IValidator<UserUpdateDto>, UserUpdateValidator>();
            services.AddScoped<FluentValidation.IValidator<DiamondShopSystem.BLL.Handlers.Product.DTOs.ProductUpdateDto>, DiamondShopSystem.BLL.Handlers.Product.Validators.ProductUpdateValidator>();
            services.AddScoped<FluentValidation.IValidator<WarrantyUpdateDto>, WarrantyUpdateValidator>();
            // Add more validators here as needed
            return services;
        }
    }
} 