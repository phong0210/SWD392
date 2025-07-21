using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Category.DTOs;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.Category.Commands.Update
{
    public class CategoryUpdateCommandHandler : IRequestHandler<CategoryUpdateCommand, CategoryInfoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryUpdateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryInfoDto> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Category>();
            var category = await repo.GetByIdAsync(request.CategoryId);
            if (category == null)
                return null;

            category.Name = request.Dto.Name;
            category.Description = request.Dto.Description;
            repo.Update(category);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CategoryInfoDto>(category);
        }
    }
}   