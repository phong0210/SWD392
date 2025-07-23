using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using AutoMapper;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Update
{
    public class OrderDetailUpdateCommandHandler : IRequestHandler<OrderDetailUpdateCommand, OrderDetailUpdateResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderDetailUpdateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OrderDetailUpdateResponse> Handle(OrderDetailUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var orderDetailRepo = _unitOfWork.Repository<OrderDetail>();

                var orderDetail = await orderDetailRepo.GetByIdAsync(request.Id);
                if (orderDetail == null)
                {
                    return new OrderDetailUpdateResponse
                    {
                        Success = false,
                        Message = "Order detail not found."
                    };
                }

                // Validate the update data
                if (request.OrderDetailUpdateDto == null)
                {
                    return new OrderDetailUpdateResponse
                    {
                        Success = false,
                        Message = "Update data is required."
                    };
                }

                // Update properties
                orderDetail.UnitPrice = request.OrderDetailUpdateDto.UnitPrice;
                orderDetail.Quantity = request.OrderDetailUpdateDto.Quantity;
                orderDetail.ProductId = request.OrderDetailUpdateDto.ProductId;

                orderDetailRepo.Update(orderDetail);
                await _unitOfWork.SaveChangesAsync();

                // Map to DTO using AutoMapper
                var dto = _mapper.Map<OrderDetailDto>(orderDetail);

                return new OrderDetailUpdateResponse
                {
                    Success = true,
                    Data = dto,
                    Message = "Order detail updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new OrderDetailUpdateResponse
                {
                    Success = false,
                    Message = $"Error updating order detail: {ex.Message}"
                };
            }
        }
    }
}
