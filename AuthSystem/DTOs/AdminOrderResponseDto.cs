namespace AuthSystem.DTOs
{
    public class AdminOrderResponseDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public int Items { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        //public List<ProductDetailDto> Products { get; set; }
    }
}
