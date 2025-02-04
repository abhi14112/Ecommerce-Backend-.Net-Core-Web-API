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
        public List<ProductModel> GetFilteredProducts(string sortBy, string category)
        {
            var products = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(category) && category.ToLower() != "all")
            {
                products = products.Where(p => p.Category.ToLower() == category.ToLower());
            }
            switch (sortBy.ToLower())
            {
                case "price-asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price-desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "name-asc":
                    products = products.OrderBy(p => p.ProductName);
                    break;
                case "name-desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                default:
                    products = products.OrderBy(p => p.Id); 
                    break;
            }
            return products.ToList();
        }

        public async Task AddProductAsync(ProductModel product)
        {
            await _context.Products.AddAsync(product);
        }
        public async Task<ProductModel>GetProductByIdAsync(int id)
        {
             return await _context.Products.FindAsync(id);
        }

        public async Task<List<ProductModel>> GetProductsByCategoryAsync(string category)
        {
            return await _context.Products.Where(p => p.Category.ToLower() == category.ToLower()).ToListAsync();
        }
        
        public void DeleteProduct(ProductModel product)
        {
            _context.Products.Remove(product);
        }
        public async Task SaveChangesAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}
