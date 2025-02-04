using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AuthSystem.Data;
using AuthSystem.Models;
using static System.Net.WebRequestMethods;
using AuthSystem.Repository.Interface;
namespace AuthSystem.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productService;
        public ProductController(ApplicationDbContext context,IProductRepository productService)
        {
            _productService = productService;
            _context = context;
        }

        [HttpGet("search/{searchkey}")]
        [Authorize]
        public IActionResult SearchProducts(string searchkey)
        {
            var products = _productService.SearchProducts(searchkey);
            return Ok(products);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult>GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("all/{sortBy}/{category}")]
        [Authorize]
        public IActionResult GetFilteredProducts(string sortBy, string category)
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
                    products = products.OrderBy(p => p.Id); // Default sorting
                    break;
            }

            return Ok(products.ToList());
        }



        [HttpPost("Add")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddProduct([FromForm] ProductModel product,IFormFile imageFile)
        {
            if(product == null)
            {
                return BadRequest("Product Data is Required");
            }
            try
            {
                if(imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    product.Image =$"https://localhost:7249/ProductImages/{uniqueFileName}";
                }
                _context.Products.Add(product);
                _context.SaveChanges();
                return Ok(new { message = "Product Added Successfully", product});
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error has occured whle adding the product: {ex.Message}");
            }
        }
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductModel product)
        {
            try
            {
                var existingProduct = _context.Products.Find(id);
                if(existingProduct == null)
                {
                    return NotFound(new { Message = "Product not found" });
                }
                if (!string.IsNullOrEmpty(product.ProductName))
                {   
                    existingProduct.ProductName = product.ProductName;
                }
                if (!string.IsNullOrEmpty(product.Description))
                {
                    existingProduct.Description = product.Description;
                }
                if (product.Price != 0)
                {
                    existingProduct.Price = product.Price;
                }
                if (!string.IsNullOrEmpty(product.Image))
                {
                    existingProduct.Image = product.Image;
                }
                if (!string.IsNullOrEmpty(product.Category))
                {
                    existingProduct.Category = product.Category;
                }
                _context.SaveChanges();
                return Ok(new { message = "Product Updated Successfully" });
            }
            catch(Exception ex)
            {
                return StatusCode(500,new {message = "An error has occured while updating the product", error = ex.Message });
            }
        }

        [HttpGet("Product/{id}")]
        public IActionResult GetSingleProduct(int id)
        {
            try
            {
                var product = _context.Products.Find(id);
                if(product == null)
                {
                    return NotFound(new { message = "Product Not Found" });
                }
                return Ok(product);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "An error has occured while fetching the product", error = ex.Message });
            }
        }

        [HttpGet("Category/{name}")]
        public IActionResult ProductByCategory(string name)
        {
            try
            {
                var products = _context.Products.Where(p => p.Category.ToLower() == name.ToLower()).ToList();
                if (products.Count == 0)
                {
                    return NotFound(new { message = "No Products Found" });
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error has occured while fetching the products", error = ex.Message });
            }
        }


        [HttpDelete("Delete/{id}")]
        [Authorize(Roles="admin")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var product = _context.Products.Find(id);
                if(product == null)
                {
                    return NotFound(new { Message = "Product not found" });
                }
                _context.Products.Remove(product);
                _context.SaveChanges();
                return Ok(new {message= "Product Deleted Successfully" }); 
            }
            catch(Exception ex)
            {
                return StatusCode(500,new {Message = "An error has occured while deleting the product", error = ex.Message });
            }
        }
    }
}
