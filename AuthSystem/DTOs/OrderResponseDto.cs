using Microsoft.Identity.Client;

namespace AuthSystem.DTOs
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public AddressDTO Address { get; set; }
        public string OrderStatus { get; set; }
        public List<ProductDetailDto> Products { get; set; }
    }
}