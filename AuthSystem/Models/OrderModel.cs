using System;
using System.ComponentModel.DataAnnotations;
namespace AuthSystem.Models
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }
        public string SessionId { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public string ProductId { get; set; }
    }
}
