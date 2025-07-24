using System;
using System.Threading.Tasks;
using System.Linq;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Handlers.Product.DTOs;
using AutoMapper;

namespace DiamondShopSystem.BLL.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task DeleteProductAsync(Guid productId, Boolean status)
        {
            var productRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Product>();
            var productEntity = productRepo.GetByIdAsync(productId);
            if (productEntity.Result == null)
            {
                return Task.FromResult(new ProductUpdateResponseDto
                {
                    Success = false,
                    Error = "Product not found"
                });
            }

            productEntity.Result.IsHidden = status;


            productRepo.Update(productEntity.Result);

            return _unitOfWork.SaveChangesAsync();

        }

        public async Task<IEnumerable<ProductGetResponseDto>> GetAllProductsAsync()
        {
            try
            {
                var productRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Product>();
                var products = await productRepo.GetAllAsync();
                if (products == null || !products.Any())
                {
                    return new List<ProductGetResponseDto>
                    {
                        new ProductGetResponseDto
                        {
                            Success = false,
                            Error = "Product not found"
                        }
                    };
                }

                var productDtos = products.Select(product => new ProductGetResponseDto
                {
                    Success = true,
                    Product = _mapper.Map<ProductInfoDto>(product)
                });

                return productDtos;
            }
            catch (Exception)
            {
                return new List<ProductGetResponseDto>
        {
            new ProductGetResponseDto
            {
                Success = false,
                Error = "An error occurred while retrieving product information."
            }
        };
            }
        }

        public async Task<ProductGetResponseDto> GetProductByIdAsync(Guid productId)
        {
            try
            {
                var productRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Product>();
                var productEntity = await productRepo.GetByIdAsync(productId);

                if (productEntity == null)
                {
                    return new ProductGetResponseDto
                    {
                        Success = false,
                        Error = "Product not found"
                    };
                }

                var productInfo = _mapper.Map<ProductInfoDto>(productEntity);

                return new ProductGetResponseDto
                {
                    Success = true,
                    Product = productInfo
                };
            }
            catch (Exception)
            {
                return new ProductGetResponseDto
                {
                    Success = false,
                    Error = "An error occurred while retrieving product information."
                };
            }
        }

        public async Task<ProductUpdateResponseDto> UpdateProductAsync(Guid productId, ProductUpdateDto updateDto)
        {
            try
            {
                var productRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Product>();
                var productEntity = await productRepo.GetByIdAsync(productId);

                if (productEntity == null)
                {
                    return new ProductUpdateResponseDto
                    {
                        Success = false,
                        Error = "Product not found"
                    };
                }

                // Update product information
                productEntity.Name = updateDto.Name.Trim();
                productEntity.SKU = updateDto.SKU;
                productEntity.Description = updateDto.Description ?? string.Empty;
                productEntity.Price = updateDto.Price;
                productEntity.Carat = updateDto.Carat;
                productEntity.Color = updateDto.Color ?? string.Empty;
                productEntity.Clarity = updateDto.Clarity ?? string.Empty;
                productEntity.Cut = updateDto.Cut ?? string.Empty;
                productEntity.StockQuantity = updateDto.StockQuantity;
                productEntity.GIACertNumber = updateDto.GIACertNumber ?? string.Empty;
                productEntity.IsHidden = updateDto.IsHidden;
                productEntity.CategoryId = updateDto.CategoryId;

                productRepo.Update(productEntity);
                await _unitOfWork.SaveChangesAsync();

                var updatedProduct = await productRepo.GetByIdAsync(productId);
                var productInfo = _mapper.Map<ProductInfoDto>(updatedProduct);

                return new ProductUpdateResponseDto
                {
                    Success = true,
                    Product = productInfo
                };
            }
            catch (Exception)
            {
                return new ProductUpdateResponseDto
                {
                    Success = false,
                    Error = "An error occurred while updating product information."
                };
            }
        }
    }
}