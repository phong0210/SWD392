using DiamondShopSystem.BLL.Handlers.Product.DTOs;
using DiamondShopSystem.BLL.Handlers.User.Commands.Update;
using DiamondShopSystem.BLL.Handlers.User.DTOs;
using DiamondShopSystem.BLL.Services.Product;
using DiamondShopSystem.BLL.Services.User;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Product.Commands.Update
{
    public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand, ProductUpdateResponseDto>
    {

        private readonly IProductService _service;
        private readonly IValidator<ProductUpdateDto> _validator;

        public ProductUpdateCommandHandler(IProductService productService, IValidator<ProductUpdateDto> validator)
        {
            _service = productService;
            _validator = validator;
        }

        public async Task<ProductUpdateResponseDto> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate the request
                var validationResult = await _validator.ValidateAsync(request.UpdateData, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return new ProductUpdateResponseDto
                    {
                        Success = false,
                        Error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                    };
                }

                // Update user account
                var result = await _service.UpdateProductAsync(request.ProductId, request.UpdateData);

                return result;
            }
            catch (Exception)
            {
                return new ProductUpdateResponseDto
                {
                    Success = false,
                    Error = "An error occurred while updating user account information."
                };
            }
        }


    }
}
