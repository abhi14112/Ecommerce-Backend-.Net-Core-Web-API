using AuthSystem.Data;
using AuthSystem.Models;
using AuthSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace AuthSystem.Repository.Services
{
    public class ProductRepository:IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<ProductModel> SearchProducts(string searchKey)
        {
            searchKey = searchKey.ToLower();
            return _context.Products.Where(p => p.ProductName.ToLower().Contains(searchKey));
        }
        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
