using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Handlers.User.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.Get
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserListDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var userRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
            var users = await userRepo.GetAllQueryable().Include(u => u.StaffProfile).ThenInclude(sp => sp.Role).ToListAsync();
            return _mapper.Map<List<UserListDto>>(users);
        }
    }
}
