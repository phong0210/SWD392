
using AutoMapper;
using DiamondShopSystem.BLL.Services.Interfaces;
using DiamondShopSystem.BLL.Domain.Entities;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;

namespace DiamondShopSystem.BLL.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<ProductResponse> CreateProductAsync(CreateProductRequest request)
        {
            var product = _mapper.Map<Products>(request);
            await _productRepository.AddAsync(product);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ProductResponse>(product);
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return false;
            }

            product.IsHidden = true;
            _productRepository.Update(product);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductResponse>>(products);
        }

        public async Task<ProductResponse> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return _mapper.Map<ProductResponse>(product);
        }

        public async Task<ProductResponse> UpdateProductAsync(Guid id, UpdateProductRequest request)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return null;
            }

            _mapper.Map(request, product);
            _productRepository.Update(product);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ProductResponse>(product);
        }
    }
}
