using System.ComponentModel.DataAnnotations;
using AuthSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
namespace AuthSystem.Models
{
    public class CartItemModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public CartModel Cart { get; set; }
        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

    }
}
