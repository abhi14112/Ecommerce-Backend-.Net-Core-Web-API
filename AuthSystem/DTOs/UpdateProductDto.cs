using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace AuthSystem.DTOs
{
    public class UpdateProductDto
    {
            public string ProductName { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string Image { get; set; }
            public string Category { get; set; }
            public int CategoryModelId { get; set; }
    }
}