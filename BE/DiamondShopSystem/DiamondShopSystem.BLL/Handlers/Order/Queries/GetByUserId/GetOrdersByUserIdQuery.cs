using MediatR;
using System;
using System.Collections.Generic;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Queries.GetByUserId
{
    public class GetOrdersByUserIdQuery : IRequest<List<OrderResponseDto>>
    {
        public Guid UserId { get; set; }
    }
}
