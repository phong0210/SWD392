using AutoMapper;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Services.LoyaltyPoint
{
    public class LoyaltyPointService : ILoyaltyPointService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoyaltyPointService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LoyaltyPointResponseDto>> GetAllLoyaltyPointsAsync()
        {
            var loyaltyPoints = await _unitOfWork.Repository<LoyaltyPoints>().GetAllAsync();
            return _mapper.Map<IEnumerable<LoyaltyPointResponseDto>>(loyaltyPoints);
        }

        public async Task<LoyaltyPointResponseDto> GetLoyaltyPointByIdAsync(Guid id)
        {
            var loyaltyPoint = await _unitOfWork.Repository<LoyaltyPoints>().GetByIdAsync(id);
            return _mapper.Map<LoyaltyPointResponseDto>(loyaltyPoint);
        }

        public async Task AddLoyaltyPointAsync(LoyaltyPointResponseDto loyaltyPointDto)
        {
            var loyaltyPoint = _mapper.Map<LoyaltyPoints>(loyaltyPointDto);
            await _unitOfWork.Repository<LoyaltyPoints>().AddAsync(loyaltyPoint);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateLoyaltyPointAsync(LoyaltyPointResponseDto loyaltyPointDto)
        {
            var loyaltyPoint = _mapper.Map<LoyaltyPoints>(loyaltyPointDto);
            _unitOfWork.Repository<LoyaltyPoints>().Update(loyaltyPoint);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteLoyaltyPointAsync(Guid id)
        {
            var loyaltyPoint = await _unitOfWork.Repository<LoyaltyPoints>().GetByIdAsync(id);
            if (loyaltyPoint != null)
            {
                _unitOfWork.Repository<LoyaltyPoints>().Remove(loyaltyPoint);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}