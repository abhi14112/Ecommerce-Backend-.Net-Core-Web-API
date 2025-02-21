using AuthSystem.Data;
using AuthSystem.DTOs;
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
                OrderStatus = OrderStatus.Received.ToString()
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

            return orders.Select(order =>
            {
                var productIds = order.ProductId.Split(',').Select(int.Parse).ToList();
                var productQuantities = order.ProductQuantity.Split(',').Select(int.Parse).ToList();

                var products = productIds.Select((productId, index) =>
                {
                    var product = productList.FirstOrDefault(p => p.Id == productId);
                    return new ProductDetailDto
                    {
                        ProductId = product.Id,
                        ProductName = product?.ProductName ?? "Unknown",
                        Price = product?.Price ?? 0,
                        Quantity = productQuantities[index],
                        ProductDescription = product?.Description ?? "Unknown",
                        ImageUrl = product?.Image ??"Unknown"
  
                    };
                }).ToList();
                return new OrderResponseDto
                {
                    Id = order.Id,
                    TotalAmount = order.TotalAmount,
                    PaymentStatus = order.PaymentStatus,
                    CreatedAt = order.CreatedAt,
                    Products = products,
                };
            }).ToList();
        }
    }
}