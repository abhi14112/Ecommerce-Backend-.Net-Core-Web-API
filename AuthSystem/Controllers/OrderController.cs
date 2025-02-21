using AuthSystem.DTOs;
using AuthSystem.Models;
using AuthSystem.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderService;
        public OrderController(IOrderRepository orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("{email}")]
        public async Task<IEnumerable<OrderResponseDto>>GetAllOrders(string email)
        {
            return await _orderService.GetCustomerOrders(email);
        }
        [HttpGet("admin")]
        public async Task<IEnumerable<AdminOrderResponseDto>> GetAdminOrdersAsync()
        {
            return await _orderService.GetAdminOrders();
        }
    }
}