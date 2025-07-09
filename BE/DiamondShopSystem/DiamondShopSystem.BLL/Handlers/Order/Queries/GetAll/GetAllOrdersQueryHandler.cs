using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using DiamondShopSystem.DAL.Repositories;

using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Queries.GetAll
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<OrderResponseDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orderRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Order>();
            var orders = await orderRepository.GetAllAsync();

            // Manually load related entities for each order since IGenericRepository does not support Include
            // This is necessary for AutoMapper to correctly map nested DTOs
            var orderDetailRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.OrderDetail>();
            var paymentRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Payment>();
            var deliveryRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Delivery>();

            foreach (var order in orders)
            {
                order.OrderDetails = (await orderDetailRepository.FindAsync(od => od.OrderId == order.Id)).ToList();
                order.Payments = (await paymentRepository.FindAsync(p => p.OrderId == order.Id)).ToList();
                order.Delivery = (await deliveryRepository.FindAsync(d => d.OrderId == order.Id)).FirstOrDefault();
            }

            return _mapper.Map<List<OrderResponseDto>>(orders.ToList());
        }
    }
}
