using AutoMapper;

using DiamondShopSystem.BLL.Handlers.Warranty.DTOs;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;
using MediatR;


namespace DiamondShopSystem.BLL.Handlers.Warranty.Commands.Create
{
    public class WarrantyCreateCommandHandler : IRequestHandler<WarrantyCreateCommand, WarrantyCreateResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WarrantyCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<WarrantyCreateResponseDto> Handle(WarrantyCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {

                // Map DTO to Entity
                var warranty = _mapper.Map<DiamondShopSystem.DAL.Entities.Warranty>(request.Dto);

                // Add to repository
                var warrentyRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Warranty>();
                await warrentyRepo.AddAsync(warranty);

                // Save changes
                await _unitOfWork.SaveChangesAsync();

                return new WarrantyCreateResponseDto
                {
                    Success = true,
                    WarrantyId = warranty.Id
                };
            }
            catch (Exception ex)
            {
                return new WarrantyCreateResponseDto
                {
                    Success = false,
                    Error = $"An error occurred while creating the product: {ex.Message}"
                };
            }
        }
    }
}