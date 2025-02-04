using AuthSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Repository.Interface
{
    public interface IProductRepository
    {
        IEnumerable<ProductModel>SearchProducts(string searchKey);
        Task<IEnumerable<ProductModel>> GetAllProductsAsync();
    }
}
