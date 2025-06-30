using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Promotions
{
    public class CreatePromotionCommand : IRequest<PromotionDto>
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ProductId { get; set; }
        public Guid StaffId { get; set; }
    }

    public class CreatePromotionCommandHandler : IRequestHandler<CreatePromotionCommand, PromotionDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePromotionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PromotionDto> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
        {
            // Validate staff member exists and has appropriate role
            var staff = await _unitOfWork.Users.GetByIdAsync(request.StaffId);
            if (staff == null)
            {
                throw new ArgumentException("Staff member not found");
            }

            // Check if staff has appropriate role (StoreManager or HeadOfficeAdmin)
            if (staff.Role?.Name != "StoreManager" && staff.Role?.Name != "HeadOfficeAdmin")
            {
                throw new UnauthorizedAccessException("Insufficient permissions to create promotions");
            }

            // Validate product exists
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new ArgumentException("Product not found");
            }

            // Validate promotion dates
            if (request.StartDate >= request.EndDate)
            {
                throw new ArgumentException("Start date must be before end date");
            }

            if (request.StartDate < DateTime.UtcNow)
            {
                throw new ArgumentException("Start date cannot be in the past");
            }

            // Validate discount percentage
            if (request.DiscountPercentage <= 0 || request.DiscountPercentage > 100)
            {
                throw new ArgumentException("Discount percentage must be between 0 and 100");
            }

            // Check if promotion code already exists
            var existingPromotions = await _unitOfWork.Promotions.ListAllAsync();
            if (existingPromotions.Any(p => p.Code == request.Code))
            {
                throw new ArgumentException("Promotion code already exists");
            }

            var promotion = new Promotion
            {
                Id = Guid.NewGuid(),
                Code = request.Code,
                Description = request.Description,
                DiscountPercentage = request.DiscountPercentage,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ProductId = request.ProductId
            };

            await _unitOfWork.Promotions.AddAsync(promotion);
            await _unitOfWork.CommitAsync();

            // Use positional constructor for PromotionDto
            return new PromotionDto(
                promotion.Id,
                promotion.Code,
                promotion.Description,
                promotion.DiscountPercentage,
                promotion.StartDate,
                promotion.EndDate,
                promotion.ProductId,
                product.Name
            );
        }
    }
}