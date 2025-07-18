using DiamondShopSystem.BLL.Handlers.Product.DTOs;
using DiamondShopSystem.BLL.Handlers.User.Commands.Update;
using DiamondShopSystem.BLL.Handlers.User.DTOs;
using DiamondShopSystem.BLL.Handlers.Warranty.Commands.Update;
using DiamondShopSystem.BLL.Handlers.Warranty.DTOs;
using DiamondShopSystem.BLL.Services.Product;
using DiamondShopSystem.BLL.Services.User;
using DiamondShopSystem.BLL.Services.Warranty;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Warranty.Commands.Update
{
    public class WarrantyUpdateCommandHandler : IRequestHandler<WarrantyUpdateCommand, WarrantyUpdateResponseDto>
    {

        private readonly IWarrantyService _service;
        private readonly IValidator<WarrantyUpdateDto> _validator;

        public WarrantyUpdateCommandHandler(IWarrantyService warrantyService, IValidator<WarrantyUpdateDto> validator)
        {
            _service = warrantyService;
            _validator = validator;
        }

        public async Task<WarrantyUpdateResponseDto> Handle(WarrantyUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate the request
                var validationResult = await _validator.ValidateAsync(request.Dto, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return new WarrantyUpdateResponseDto
                    {
                        Success = false,
                        Error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                    };
                }

                // Update user account
                var result = await _service.UpdateWarrantyAsync(request.WarrantyId, request.Dto);

                return result;
            }
            catch (Exception)
            {
                return new WarrantyUpdateResponseDto
                {
                    Success = false,
                    Error = "An error occurred while updating user account information."
                };
            }
        }


    }
}
