using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Category.DTOs;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.Category.Commands.Get
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryInfoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryInfoDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<DAL.Entities.Category>();
            var category = await repo.GetByIdAsync(request.CategoryId);
            return category == null ? null : _mapper.Map<CategoryInfoDto>(category);
        }
    }
}