using MediatR;
using System.Collections.Generic;
using DiamondShopSystem.BLL.Handlers.User.DTOs;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.Get
{
    public class GetAllUsersQuery : IRequest<List<UserListDto>>
    {
    }
}
