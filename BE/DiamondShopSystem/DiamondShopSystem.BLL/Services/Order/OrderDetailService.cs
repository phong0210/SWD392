using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using DiamondShopSystem.DAL;
using DiamondShopSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiamondShopSystem.BLL.Services.Order
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly AppDbContext _context;
        public OrderDetailService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDetailDto> CreateOrderDetailAsync(OrderDetailCreateDto dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);
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
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
            return MapToDto(orderDetail);
        }

        public async Task<OrderDetailDto?> GetOrderDetailByIdAsync(Guid id)
        {
            var orderDetail = await _context.OrderDetails
                .Include(od => od.Product)
                .FirstOrDefaultAsync(od => od.Id == id);
            return orderDetail == null ? null : MapToDto(orderDetail);
        }

        public async Task<IEnumerable<OrderDetailDto>> GetAllOrderDetailsAsync()
        {
            var orderDetails = await _context.OrderDetails
                .Include(od => od.Product)
                .ToListAsync();
            return orderDetails.Select(MapToDto);
        }

        public async Task<IEnumerable<OrderDetailDto>> GetOrderDetailsByOrderIdAsync(Guid orderId)
        {
            var orderDetails = await _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
            return orderDetails.Select(MapToDto);
        }

        public async Task<OrderDetailDto> UpdateOrderDetailAsync(Guid id, OrderDetailUpdateDto dto)
        {
            var orderDetail = await _context.OrderDetails
                .Include(od => od.Product)
                .FirstOrDefaultAsync(od => od.Id == id);
            if (orderDetail == null) throw new Exception("OrderDetail not found");
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null) throw new Exception("Product not found");
            orderDetail.UnitPrice = dto.UnitPrice;
            orderDetail.Quantity = dto.Quantity;
            orderDetail.ProductId = dto.ProductId;
            orderDetail.Product = product;
            await _context.SaveChangesAsync();
            return MapToDto(orderDetail);
        }

        public async Task<bool> DeleteOrderDetailAsync(Guid id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null) return false;
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
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