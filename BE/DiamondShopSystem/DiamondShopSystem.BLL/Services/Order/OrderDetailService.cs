using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using DiamondShopSystem.DAL;
using DiamondShopSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using DiamondShopSystem.DAL.Repositories;

namespace DiamondShopSystem.BLL.Services.Order
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderDetailDto> CreateOrderDetailAsync(OrderDetailCreateDto dto)
        {
            var productRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Product>();
            var orderDetailRepo = _unitOfWork.Repository<OrderDetail>();
            var products = await productRepo.FindAsync(p => p.Id == dto.ProductId);
            var product = products.FirstOrDefault();
            if (product == null) throw new Exception("Product not found");
            var orderDetail = new OrderDetail
            {
                Id = Guid.NewGuid(),
                OrderId = dto.OrderId,
                UnitPrice = dto.UnitPrice,
                Quantity = dto.Quantity,
                ProductId = dto.ProductId,
                Product = product
            };
            await orderDetailRepo.AddAsync(orderDetail);
            await _unitOfWork.SaveChangesAsync();
            return MapToDto(orderDetail);
        }

        public async Task<OrderDetailDto?> GetOrderDetailByIdAsync(Guid id)
        {
            var orderDetailRepo = _unitOfWork.Repository<OrderDetail>();
            var orderDetails = await orderDetailRepo.FindAsync(od => od.Id == id);
            var orderDetail = orderDetails.FirstOrDefault();
            if (orderDetail == null) return null;
            var productRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Product>();
            var products = await productRepo.FindAsync(p => p.Id == orderDetail.ProductId);
            orderDetail.Product = products.FirstOrDefault()!;
            return MapToDto(orderDetail);
        }

        public async Task<IEnumerable<OrderDetailDto>> GetAllOrderDetailsAsync()
        {
            var orderDetailRepo = _unitOfWork.Repository<OrderDetail>();
            var productRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Product>();
            var orderDetails = await orderDetailRepo.GetAllAsync();
            foreach (var od in orderDetails)
            {
                var products = await productRepo.FindAsync(p => p.Id == od.ProductId);
                od.Product = products.FirstOrDefault()!;
            }
            return orderDetails.Select(MapToDto);
        }

        public async Task<IEnumerable<OrderDetailDto>> GetOrderDetailsByOrderIdAsync(Guid orderId)
        {
            var orderDetailRepo = _unitOfWork.Repository<OrderDetail>();
            var productRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Product>();
            var orderDetails = await orderDetailRepo.FindAsync(od => od.OrderId == orderId);
            foreach (var od in orderDetails)
            {
                var products = await productRepo.FindAsync(p => p.Id == od.ProductId);
                od.Product = products.FirstOrDefault()!;
            }
            return orderDetails.Select(MapToDto);
        }

        public async Task<OrderDetailDto> UpdateOrderDetailAsync(Guid id, OrderDetailUpdateDto dto)
        {
            var orderDetailRepo = _unitOfWork.Repository<OrderDetail>();
            var productRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.Product>();
            var orderDetails = await orderDetailRepo.FindAsync(od => od.Id == id);
            var orderDetail = orderDetails.FirstOrDefault();
            if (orderDetail == null) throw new Exception("OrderDetail not found");
            var products = await productRepo.FindAsync(p => p.Id == dto.ProductId);
            var product = products.FirstOrDefault();
            if (product == null) throw new Exception("Product not found");
            orderDetail.UnitPrice = dto.UnitPrice;
            orderDetail.Quantity = dto.Quantity;
            orderDetail.ProductId = dto.ProductId;
            orderDetail.Product = product;
            // No explicit update method in repo, assume tracked entity
            await _unitOfWork.SaveChangesAsync();
            return MapToDto(orderDetail);
        }

        public async Task<bool> DeleteOrderDetailAsync(Guid id)
        {
            var orderDetailRepo = _unitOfWork.Repository<OrderDetail>();
            var orderDetails = await orderDetailRepo.FindAsync(od => od.Id == id);
            var orderDetail = orderDetails.FirstOrDefault();
            if (orderDetail == null) return false;
            orderDetailRepo.Remove(orderDetail);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static OrderDetailDto MapToDto(OrderDetail orderDetail)
        {
            return new OrderDetailDto
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                UnitPrice = orderDetail.UnitPrice,
                Quantity = orderDetail.Quantity,
                ProductId = orderDetail.ProductId,
                Product = orderDetail.Product != null ? new ProductDto
                {
                    Id = orderDetail.Product.Id,
                    Name = orderDetail.Product.Name,
                    SKU = orderDetail.Product.SKU,
                    Description = orderDetail.Product.Description,
                    Price = orderDetail.Product.Price
                } : null!
            };
        }
    }
} 