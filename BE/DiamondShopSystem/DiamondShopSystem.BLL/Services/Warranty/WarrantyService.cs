using AutoMapper;
using DiamondShopSystem.BLL.Handlers.Warranty.DTOs;
using DiamondShopSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Services.Warranty
{
    public class WarrantyService : IWarrantyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WarrantyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task DeleteWarrantyAsync(Guid id)
        {
            var warrantyRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Warranty>();
            var warrantyEntity = await warrantyRepo.GetByIdAsync(id);
            if (warrantyEntity == null)
                return;
            warrantyRepo.Remove(warrantyEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<WarrantyGetResponseDto>> GetAllWarrantiesAsync()
        {
            var warrantyRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Warranty>();
            var warranties = await warrantyRepo.GetAllAsync();
            if (warranties == null || !warranties.Any())
            {
                return new List<WarrantyGetResponseDto>
                {
                    new WarrantyGetResponseDto
                    {
                        Success = false,
                        Error = "No warranties found"
                    }
                };
            }
            var warrantyDtos = warranties.Select(warranty => new WarrantyGetResponseDto
            {
                Success = true,
                Warranty = _mapper.Map<WarrantyInfoDto>(warranty)
            });
            return warrantyDtos;
        }

        public async Task<WarrantyGetResponseDto> GetWarrantyByIdAsync(Guid id)
        {
            var warrantyRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Warranty>();
            var warrantyEntity = await warrantyRepo.GetByIdAsync(id);
            if (warrantyEntity == null)
            {
                return new WarrantyGetResponseDto
                {
                    Success = false,
                    Error = "Warranty not found"
                };
            }
            return new WarrantyGetResponseDto
            {
                Success = true,
                Warranty = _mapper.Map<WarrantyInfoDto>(warrantyEntity)
            };
        }

        public async Task<WarrantyUpdateResponseDto> UpdateWarrantyAsync(Guid id, WarrantyUpdateDto warranty)
        {
            var warrantyRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Warranty>();
            var warrantyEntity = await warrantyRepo.GetByIdAsync(id);
            if (warrantyEntity == null)
            {
                return new WarrantyUpdateResponseDto
                {
                    Success = false,
                    Error = "Warranty not found"
                };
            }
            warrantyEntity.ProductId = warranty.ProductId;
            warrantyEntity.WarrantyStart = warranty.WarrantyStart;
            warrantyEntity.WarrantyEnd = warranty.WarrantyEnd;
            warrantyEntity.Details = warranty.Details;
            warrantyEntity.IsActive = warranty.IsActive;
            warrantyRepo.Update(warrantyEntity);
            await _unitOfWork.SaveChangesAsync();

            var updatedWarranty = await warrantyRepo.GetByIdAsync(id);
            if (updatedWarranty == null)
            {
                return new WarrantyUpdateResponseDto
                {
                    Success = false,
                    Error = "Failed to update warranty"
                };
            }
            return new WarrantyUpdateResponseDto
            {
                Success = true,
                Warranty = _mapper.Map<WarrantyInfoDto>(updatedWarranty)
            };
        }
    }
}