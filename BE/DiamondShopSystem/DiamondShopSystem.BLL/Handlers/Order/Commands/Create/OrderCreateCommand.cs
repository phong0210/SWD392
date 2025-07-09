using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Order.Commands.Create
{
    public class OrderCreateCommand : IRequest<CreateOrderResponseDto>
    {
        public CreateOrderDto Dto { get; }

        public OrderCreateCommand(CreateOrderDto dto)
        {
            Dto = dto;
        }
    }
}
