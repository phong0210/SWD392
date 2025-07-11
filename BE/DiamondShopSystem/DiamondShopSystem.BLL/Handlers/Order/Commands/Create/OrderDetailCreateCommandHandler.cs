using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Create;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using DiamondShopSystem.DAL;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Services.Order;
using AutoMapper;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Create
{
    public class OrderDetailCreateCommandHandler : IRequestHandler<OrderDetailCreateCommand, OrderDetailCreateResponse>
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly IMapper _mapper;

        public OrderDetailCreateCommandHandler(IOrderDetailService orderDetailService, IMapper mapper)
        {
            _orderDetailService = orderDetailService;
            _mapper = mapper;
        }

        public async Task<OrderDetailCreateResponse> Handle(OrderDetailCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var orderDetail = await _orderDetailService.CreateOrderDetailAsync(request.OrderDetailCreateDto);
                
                return new OrderDetailCreateResponse
                {
                    Success = true,
                    Message = "Order detail created successfully",
                    Data = orderDetail
                };
            }
            catch (Exception ex)
            {
                return new OrderDetailCreateResponse
                {
                    Success = false,
                    Message = $"Failed to create order detail: {ex.Message}"
                };
            }
        }
    }
} 