using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AuthSystem.Models
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }
        public string? SessionId { get; set; }
        [Required]
        public string? CustomerEmail { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public string? ProductId { get; set; }
        [Required]
        public string? ProductQuantity { get; set; }
        public string Address { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    }
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled,
        Returned
    }
}
