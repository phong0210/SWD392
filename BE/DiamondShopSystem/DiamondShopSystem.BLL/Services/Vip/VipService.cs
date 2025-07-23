using System;
using System.Threading.Tasks;
using System.Linq;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using DiamondShopSystem.BLL.Interfaces;
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

        public async Task<VipGetResponseDto> GetVipByIdAsync(Guid vipId)
        {
            try
            {
                var vipRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Vip>();
                var vipEntity = await vipRepo.GetByIdAsync(vipId);

                if (vipEntity == null)
                {
                    return new VipGetResponseDto
                    {
                        Success = false,
                        Error = "Vip not found"
                    };
                }

                var vipDto = _mapper.Map<VipDto>(vipEntity);

                return new VipGetResponseDto
                {
                    Success = true,
                    Vip = vipDto
                };
            }
            catch (Exception ex)
            {
                return new VipGetResponseDto
                {
                    Success = false,
                    Error = $"An error occurred while retrieving vip information: {ex.Message}"
                };
            }
        }

        public async Task<VipListResponseDto> GetAllVipsAsync()
        {
            try
            {
                var vipRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Vip>();
                var vipEntities = await vipRepo.GetAllAsync();

                var vipDtos = _mapper.Map<List<VipDto>>(vipEntities);

                return new VipListResponseDto
                {
                    Success = true,
                    Vips = vipDtos
                };
            }
            catch (Exception ex)
            {
                return new VipListResponseDto
                {
                    Success = false,
                    Error = $"An error occurred while retrieving all vips: {ex.Message}"
                };
            }
        }

        public async Task<VipCreateResponseDto> CreateVipAsync(VipCreateRequestDto createDto)
        {
            try
            {
                var vipRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Vip>();
                var vipEntity = _mapper.Map<DiamondShopSystem.DAL.Entities.Vip>(createDto);

                vipEntity.VipId = Guid.NewGuid();
                vipEntity.StartDate = DateTime.UtcNow;

                await vipRepo.AddAsync(vipEntity);
                await _unitOfWork.SaveChangesAsync();

                var vipDto = _mapper.Map<VipDto>(vipEntity);

                return new VipCreateResponseDto
                {
                    Success = true,
                    Vip = vipDto
                };
            }
            catch (Exception ex)
            {
                return new VipCreateResponseDto
                {
                    Success = false,
                    Error = $"An error occurred while creating vip: {ex.Message}"
                };
            }
        }

        public async Task<VipUpdateResponseDto> UpdateVipAsync(Guid vipId, VipUpdateRequestDto updateDto)
        {
            try
            {
                var vipRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Vip>();
                var vipEntity = await vipRepo.GetByIdAsync(vipId);

                if (vipEntity == null)
                {
                    return new VipUpdateResponseDto
                    {
                        Success = false,
                        Error = "Vip not found"
                    };
                }

                _mapper.Map(updateDto, vipEntity);

                vipRepo.Update(vipEntity);
                await _unitOfWork.SaveChangesAsync();

                var vipDto = _mapper.Map<VipDto>(vipEntity);

                return new VipUpdateResponseDto
                {
                    Success = true,
                    Vip = vipDto
                };
            }
            catch (Exception ex)
            {
                return new VipUpdateResponseDto
                {
                    Success = false,
                    Error = $"An error occurred while updating vip: {ex.Message}"
                };
            }
        }

        public async Task<VipDeleteResponseDto> DeleteVipAsync(Guid vipId)
        {
            try
            {
                var vipRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Vip>();
                var vipEntity = await vipRepo.GetByIdAsync(vipId);

                if (vipEntity == null)
                {
                    return new VipDeleteResponseDto
                    {
                        Success = false,
                        Error = "Vip not found"
                    };
                }

                vipRepo.Delete(vipEntity);
                await _unitOfWork.SaveChangesAsync();

                return new VipDeleteResponseDto
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new VipDeleteResponseDto
                {
                    Success = false,
                    Error = $"An error occurred while deleting vip: {ex.Message}"
                };
            }
        }
    }
}