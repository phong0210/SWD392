using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Category.DTOs;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.Category.Commands.Create
{
    public class CategoryCreateCommandHandler : IRequestHandler<CategoryCreateCommand, CategoryInfoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryInfoDto> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<DiamondShopSystem.DAL.Entities.Category>(request.Dto);
            category.Id = Guid.NewGuid();
            var repo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Category>();
            await repo.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CategoryInfoDto>(category);
        }
    }
}   