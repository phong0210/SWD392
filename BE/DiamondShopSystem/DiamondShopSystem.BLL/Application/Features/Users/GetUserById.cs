using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiamondShopSystem.BLL.Application.Features.Users
{
    public record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;

    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdWithRoleAsync(request.Id);
            if (user == null) return null;
            return _mapper.Map<UserDto>(user);
        }
    }
} 