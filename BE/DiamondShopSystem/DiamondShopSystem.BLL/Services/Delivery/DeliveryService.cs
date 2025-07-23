using DiamondShopSystem.BLL.Handlers.Delivery.DTOs;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Services.Delivery
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeliveryService(IDeliveryRepository deliveryRepository, IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _deliveryRepository = deliveryRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DeliveryDto>> GetAllDeliveriesAsync()
        {
            var deliveries = await _deliveryRepository.GetAllAsync();
            return deliveries.Select(d => new DeliveryDto
            {
                Id = d.Id,
                OrderId = d.OrderId,
                DispatchTime = d.DispatchTime,
                DeliveryTime = d.DeliveryTime,
                ShippingAddress = d.ShippingAddress,
                Status = d.Status
            });
        }

        public async Task<DeliveryDto> GetDeliveryByIdAsync(Guid id)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(id);
            if (delivery == null) return null;
            return new DeliveryDto
            {
                Id = delivery.Id,
                OrderId = delivery.OrderId,
                DispatchTime = delivery.DispatchTime,
                DeliveryTime = delivery.DeliveryTime,
                ShippingAddress = delivery.ShippingAddress,
                Status = delivery.Status
            };
        }

        public async Task<CreateDeliveryDto> CreateDeliveryAsync(CreateDeliveryDto createDeliveryDto)
        {
            var order = await _orderRepository.GetByIdAsync(createDeliveryDto.OrderId);
            if (order == null)
            {
                return null; // Order does not exist
            }

            var existingDeliveries = await _deliveryRepository.FindAsync(d => d.OrderId == createDeliveryDto.OrderId);
            if (existingDeliveries.Any())
            {
                return null; // Delivery for this order already exists
            }

            var delivery = new DAL.Entities.Delivery
            {
                OrderId = createDeliveryDto.OrderId,
                ShippingAddress = createDeliveryDto.ShippingAddress,
                Status = createDeliveryDto.Status
            };

            await _deliveryRepository.AddAsync(delivery);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
            {
                return null;
            }

            return createDeliveryDto;
        }

        public async Task UpdateDeliveryAsync(Guid id, UpdateDeliveryDto updateDeliveryDto)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(id);
            if (delivery == null) return;

            delivery.DispatchTime = updateDeliveryDto.DispatchTime;
            delivery.DeliveryTime = updateDeliveryDto.DeliveryTime;
            delivery.ShippingAddress = updateDeliveryDto.ShippingAddress;
            delivery.Status = updateDeliveryDto.Status;

            _deliveryRepository.Update(delivery);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteDeliveryAsync(Guid id)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(id);
            if (delivery == null) return;

            _deliveryRepository.Remove(delivery);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}