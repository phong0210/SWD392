using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Delete
{
    public class OrderDetailDeleteCommandHandler : IRequestHandler<OrderDetailDeleteCommand, OrderDetailDeleteResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailDeleteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<OrderDetailDeleteResponse> Handle(OrderDetailDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var orderDetailRepo = _unitOfWork.Repository<OrderDetail>();

                var orderDetail = await orderDetailRepo.GetByIdAsync(request.Id);
                if (orderDetail == null)
                {
                    return new OrderDetailDeleteResponse
                    {
                        Success = false,
                        Message = "Order detail not found."
                    };
                }

                orderDetailRepo.Remove(orderDetail);
                await _unitOfWork.SaveChangesAsync();

                return new OrderDetailDeleteResponse
                {
                    Success = true,
                    Message = "Order detail deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new OrderDetailDeleteResponse
                {
                    Success = false,
                    Message = $"Error deleting order detail: {ex.Message}"
                };
            }
        }
    }
}