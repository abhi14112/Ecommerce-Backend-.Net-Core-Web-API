using AuthSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Repository.Interface
{
    public interface IProductRepository
    {
        Task<IActionResult> SearchProducts(string searchKey);
        Task<IActionResult> GetAllProducts();
        Task<IActionResult> GetFilteredProducts(string sortBy, string category);
        Task<IActionResult> AddProduct(ProductModel product, IFormFile imageFile);
        Task<IActionResult> UpdateProduct(int id, ProductModel product);
        Task<IActionResult> GetSingleProduct(int id);
        Task<IActionResult> ProductByCategory(string name);
        public IActionResult DeleteProduct(int id);
    }
}
