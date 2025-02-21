using AuthSystem.DTOs;
namespace AuthSystem.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderResponseDto>> GetCustomerOrders(string email);
        Task<IEnumerable<AdminOrderResponseDto>> GetAdminOrders();
    }
}
