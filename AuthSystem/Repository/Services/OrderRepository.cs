using AuthSystem.Data;
using AuthSystem.DTOs;
using AuthSystem.Models;
using AuthSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;
namespace AuthSystem.Repository.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IProductRepository _productService;
        private readonly ApplicationDbContext _context;

        public OrderRepository(IProductRepository productService,ApplicationDbContext context)
        {
            _productService = productService;
            _context = context;
        }
        public async Task UpdateOrderStatus(UpdateOrderStatusDto orderStatus)
        {
            int id = orderStatus.OrderId;
            var orderData =await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderStatus.OrderId);
            if(orderData != null)
            {
                Enum.TryParse<OrderStatus>(orderStatus.Status, true, out OrderStatus result);
                orderData.OrderStatus = result;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<AdminOrderResponseDto>> GetAdminOrders()
        {
            var orders = await _context.Orders
                        .OrderByDescending(o => o.CreatedAt)
                        .ToListAsync();
            var orderDtos = orders.Select(o => new AdminOrderResponseDto
            {
                Id = o.Id,
                TotalAmount = o.TotalAmount,
                Items = o.ProductQuantity.Split(',').Select(int.Parse).Sum(),
                PaymentStatus = o.PaymentStatus,
                CreatedAt = o.CreatedAt,
                OrderStatus = o.OrderStatus.ToString()
            });
            return orderDtos;
        }
        public async Task<IEnumerable<OrderResponseDto>> GetCustomerOrders(string email)
        {
            var orders = await _context.Orders
                .Where(o => o.CustomerEmail == email)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var productList = await _productService.GetAllProductsAsync();


            var addressIds = orders.Select(o => int.Parse(o.Address)).Distinct().ToList();

   
            var addressMap = await _context.Addressess
                .Where(a => addressIds.Contains(a.Id))
                .ToDictionaryAsync(a => a.Id);

            var result = orders.Select(order =>
            {
                var productIds = order.ProductId.Split(',').Select(int.Parse).ToList();
                var productQuantities = order.ProductQuantity.Split(',').Select(int.Parse).ToList();

                var products = productIds.Select((productId, index) =>
                {
                    var product = productList.FirstOrDefault(p => p.Id == productId);
                    return new ProductDetailDto
                    {
                        ProductId = product?.Id ?? 0,
                        ProductName = product?.ProductName ?? "Unknown",
                        Price = product?.Price ?? 0,
                        Quantity = productQuantities.ElementAtOrDefault(index),
                        ProductDescription = product?.Description ?? "Unknown",
                        ImageUrl = product?.Image ?? "Unknown"
                    };
                }).ToList();
                var addressId = int.Parse(order.Address);
                addressMap.TryGetValue(addressId, out var address);
                var addressDto = new AddressDTO
                {
                    Id = address?.Id ?? 0,
                    AddressLine = address?.AddressLine ?? "Unknown",
                    City = address?.City ?? "Unknown",
                    State = address?.State ?? "Unknown",
                    PinCode = address?.PinCode ?? "Unknown",
                    Country = address?.Country ?? "Unknown"
                };

                return new OrderResponseDto
                {
                    Id = order.Id,
                    TotalAmount = order.TotalAmount,
                    PaymentStatus = order.PaymentStatus,
                    CreatedAt = order.CreatedAt,
                    Products = products,
                    Address = addressDto,
                    OrderStatus = order.OrderStatus.ToString()
                };
            });
            return result.ToList();
        }

    }
}