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

        public async Task<List<VipDto>> GetAllVipsAsync()
        {
            var vipRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Vip>();
            var vipEntities = await vipRepo.GetAllAsync();
            return _mapper.Map<List<VipDto>>(vipEntities);
        }

        public async Task<VipDto> CreateVipAsync(VipCreateRequestDto createDto)
        {
            var vipRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Vip>();
            var vipEntity = _mapper.Map<DiamondShopSystem.DAL.Entities.Vip>(createDto);

            vipEntity.VipId = Guid.NewGuid();
            vipEntity.StartDate = DateTime.UtcNow;

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
                return null; // Or throw an exception
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