using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Category.DTOs;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.Category.Commands.Get
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryInfoDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryInfoDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<DAL.Entities.Category>();
            var categories = await repo.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryInfoDto>>(categories);
        }
    }
}