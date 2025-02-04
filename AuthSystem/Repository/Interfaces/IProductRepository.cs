using AuthSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Repository.Interface
{
    public interface IProductRepository
    {
        IEnumerable<ProductModel>SearchProducts(string searchKey);
        Task<IEnumerable<ProductModel>> GetAllProductsAsync();
        List<ProductModel> GetFilteredProducts(string sortBy, string category);
        Task AddProductAsync(ProductModel product);
        Task SaveChangesAsync();
        Task<ProductModel> GetProductByIdAsync(int id);
        Task<List<ProductModel>>GetProductsByCategoryAsync(string category);
        void DeleteProduct(ProductModel product);
    }
}
