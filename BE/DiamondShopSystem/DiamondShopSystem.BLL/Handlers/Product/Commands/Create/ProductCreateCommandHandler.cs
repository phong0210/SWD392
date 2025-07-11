using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Product.DTOs;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Product.Commands.Create
{
    public class ProductCreateCommandHandler : IRequestHandler<ProductCreateCommand, ProductCreateResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductCreateResponseDto> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {

                // Map DTO to Entity
                var product = _mapper.Map<DiamondShopSystem.DAL.Entities.Product>(request.Dto);

                // Add to repository
                var productRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Product>();
                await productRepo.AddAsync(product);

                // Save changes
                await _unitOfWork.SaveChangesAsync();

                return new ProductCreateResponseDto
                {
                    Success = true,
                    ProductId = product.Id
                };
            }
            catch (Exception ex)
            {
                return new ProductCreateResponseDto
                {
                    Success = false,
                    Error = $"An error occurred while creating the product: {ex.Message}"
                };
            }
        }
    }
}