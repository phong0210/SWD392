using MediatR;
using System;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Queries.GetRelations
{
    public class GetOrderRelationsQuery : IRequest<GetOrderRelationsResponseDto>
    {
        public Guid Id { get; set; }

        public GetOrderRelationsQuery(Guid id)
        {
            Id = id;
        }
    }
}