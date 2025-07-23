using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using DiamondShopSystem.BLL.Handlers.Auth.DTOs;
using DiamondShopSystem.BLL.Handlers.Auth.Validators;
using DiamondShopSystem.BLL.Handlers.User.DTOs;
using DiamondShopSystem.BLL.Handlers.User.Commands.Get;
using DiamondShopSystem.BLL.Handlers.User.Validators;
using DiamondShopSystem.BLL.Handlers.Product.Validators;
using DiamondShopSystem.BLL.Handlers.Warranty.DTOs;
using DiamondShopSystem.BLL.Handlers.Delivery.Validators;
using DiamondShopSystem.BLL.Handlers.Delivery.DTOs;

namespace DiamondShopSystem.BLL.Handlers
{
    public static class ValidationServiceRegistration
    {
        public static IServiceCollection AddDiamondShopValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<LoginRequestDto>, LoginRequestValidator>();
            services.AddScoped<IValidator<UserRegisterDto>, UserCreateValidator>();
            services.AddScoped<IValidator<UserGetCommand>, UserGetValidator>();
            services.AddScoped<IValidator<UserUpdateDto>, UserUpdateValidator>();
            services.AddScoped<IValidator<Product.DTOs.ProductUpdateDto>, ProductUpdateValidator>();
            services.AddScoped<IValidator<WarrantyUpdateDto>, WarrantyUpdateValidator>();
            services.AddScoped<IValidator<CreateDeliveryDto>, CreateDeliveryDtoValidator>();
            services.AddScoped<IValidator<UpdateDeliveryDto>, UpdateDeliveryDtoValidator>();
            // Add more validators here as needed
            return services;
        }
    }
} 