namespace AuthSystem.Models
{
    public class CategoryModel
    {
        public int CategoryModelId { get; set;}
        public string CategoryName { get; set; }
        public string? CategoryImage { get; set; }
        public ICollection<ProductModel> Products { get; set; }
        public bool? Status { get; set; } = false;
    }
}
