using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using AutoMapper;

namespace DiamondShopSystem.BLL.Services.Vip
{
    public class VipService : IVipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VipDto> GetVipByIdAsync(Guid vipId)
        {
            var vipRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Vip>();
            var vipEntity = await vipRepo.GetByIdAsync(vipId);
            return _mapper.Map<VipDto>(vipEntity);
        }

        public async Task<VipDto> GetVipByUserIdAsync(Guid userId)
        {
            var vipRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Vip>();
            var vipEntities = await vipRepo.FindAsync(v => v.UserId == userId);
            var vipEntity = vipEntities.FirstOrDefault();
            return _mapper.Map<VipDto>(vipEntity);
        }

        public async Task<List<VipDto>> GetAllVipsAsync()
        {
            var vipRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Vip>();
            var vipEntities = await vipRepo.GetAllAsync();
            return _mapper.Map<List<VipDto>>(vipEntities);
        }

        public async Task<VipDto> CreateVipAsync(VipCreateRequestDto createDto)
        {
            var userRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
            var userExists = await userRepo.GetByIdAsync(createDto.UserId.GetValueOrDefault());

            if (userExists == null)
            {
                throw new KeyNotFoundException($"User with ID {createDto.UserId} not found.");
            }

            var vipRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Vip>();
            var vipEntity = _mapper.Map<DiamondShopSystem.DAL.Entities.Vip>(createDto);
            vipEntity.VipId = Guid.NewGuid();
            vipEntity.StartDate = DateTime.UtcNow;
            vipEntity.EndDate = DateTime.UtcNow.AddMonths(1);
            await vipRepo.AddAsync(vipEntity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<VipDto>(vipEntity);
        }

        public async Task<VipDto> UpdateVipAsync(Guid vipId, VipUpdateRequestDto updateDto)
        {
            var vipRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Vip>();
            var vipEntity = await vipRepo.GetByIdAsync(vipId);
            if (vipEntity == null)
            {
                return null;
            }
            _mapper.Map(updateDto, vipEntity);
            vipRepo.Update(vipEntity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<VipDto>(vipEntity);
        }

        public async Task<bool> DeleteVipAsync(Guid vipId)
        {
            var vipRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Vip>();
            var vipEntity = await vipRepo.GetByIdAsync(vipId);
            if (vipEntity == null)
            {
                return false;
            }
            vipRepo.Remove(vipEntity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}