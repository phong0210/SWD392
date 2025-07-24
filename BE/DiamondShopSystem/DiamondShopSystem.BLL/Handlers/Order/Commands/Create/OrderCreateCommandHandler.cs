using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Create
{
    public class OrderCreateCommandHandler : IRequestHandler<OrderCreateCommand, CreateOrderResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateOrderResponseDto> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
        {
            var orderRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Order>();
            var productRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Product>();

            // Basic validation: Check if customer exists
            var customer = await _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>().GetByIdAsync(request.Dto.CustomerId);
            if (customer == null)
            {
                return new CreateOrderResponseDto { Success = false, Error = "Customer not found." };
            }

            // Map the DTO to the Order entity
            var order = new DiamondShopSystem.DAL.Entities.Order
            {
                Id = Guid.NewGuid(),
                UserId = request.Dto.CustomerId,
                OrderDate = DateTime.UtcNow,
                SaleStaff = request.Dto.SaleStaff,
                Status = 0, // Default status, e.g., "Pending"
                VipApplied = false, // Logic for this would go here
                TotalPrice = request.Dto.TotalPrice
            };

            // Create OrderDetail and Payment entities
            foreach (var itemDto in request.Dto.OrderItems)
            {
                var product = await productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                {
                    return new CreateOrderResponseDto { Success = false, Error = $"Product with ID {itemDto.ProductId} not found." };
                }

                var orderDetail = new DiamondShopSystem.DAL.Entities.OrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    ProductId = itemDto.ProductId,
                };
                order.OrderDetails.Add(orderDetail);
            }

            var payment = new DiamondShopSystem.DAL.Entities.Payment
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                Method = request.Dto.PaymentMethod,
                Amount = order.TotalPrice,
                Status = 0, // Default status, e.g., "Pending"
                Date = DateTime.UtcNow
            };
            order.Payments.Add(payment);

            await orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return new CreateOrderResponseDto
            {
                Success = true,
                OrderId = order.Id
            };
        }
    }
}