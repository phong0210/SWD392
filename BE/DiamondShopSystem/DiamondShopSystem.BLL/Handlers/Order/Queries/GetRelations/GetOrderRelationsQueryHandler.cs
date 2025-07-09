using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using DiamondShopSystem.BLL.Handlers.User.DTOs; // Added this using statement

namespace DiamondShopSystem.BLL.Handlers.Order.Queries.GetRelations
{
    public class GetOrderRelationsQueryHandler : IRequestHandler<GetOrderRelationsQuery, GetOrderRelationsResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrderRelationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetOrderRelationsResponseDto> Handle(GetOrderRelationsQuery request, CancellationToken cancellationToken)
        {
            var orderRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Order>();
            var order = await orderRepository.GetByIdAsync(request.Id);

            if (order == null)
            {
                return new GetOrderRelationsResponseDto { Success = false, Error = "Order not found." };
            }

            var userRepository = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
            var user = await userRepository.GetByIdAsync(order.UserId);

            if (user == null)
            {
                return new GetOrderRelationsResponseDto { Success = false, Error = "User not found for this order." };
            }

            // Map the User entity to UserDto to break the circular reference
            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Status = user.Status,
                CreatedAt = user.CreatedAt
            };

            return new GetOrderRelationsResponseDto { Success = true, User = userDto };
        }
    }
}