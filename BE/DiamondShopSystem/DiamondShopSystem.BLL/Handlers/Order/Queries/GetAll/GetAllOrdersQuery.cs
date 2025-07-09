using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using System.Collections.Generic;

namespace DiamondShopSystem.BLL.Handlers.Order.Queries.GetAll
{
    public class GetAllOrdersQuery : IRequest<List<OrderResponseDto>>
    {
        // No parameters needed for getting all orders
    }
}
