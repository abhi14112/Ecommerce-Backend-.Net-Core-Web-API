using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthSystem.Data;
using AuthSystem.Models;
using AuthSystem.Repository.Interface;
namespace AuthSystem.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productService;
        private readonly IProductRepository _productRepository;
        public ProductController(ApplicationDbContext context,IProductRepository productService)
        {
            _productService = productService;
        }
        [HttpGet("search/{searchkey}")]
        [Authorize]
        public IActionResult SearchProducts(string searchkey)
        {
            var products = _productService.SearchProducts(searchkey);
            return Ok(products);
        }
        [HttpGet("all")]
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
            var products = _productService.GetFilteredProducts(sortBy, category);
            return Ok(products);
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
                await _productService.AddProductAsync(product);
                await _productService.SaveChangesAsync();
                return Ok(new { message = "Product Added Successfully", product});
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error has occured whle adding the product: {ex.Message}");
            }
        }
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductModel product)
        {
            try
            {
                var existingProduct = await _productService.GetProductByIdAsync(id);
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
                await _productService.SaveChangesAsync();
                return Ok(new { message = "Product Updated Successfully" });
            }
            catch(Exception ex)
            {
                return StatusCode(500,new {message = "An error has occured while updating the product", error = ex.Message });
            }
        }

        [HttpGet("Item/{id}")]
        public async Task<IActionResult> GetSingleProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
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
        public async Task<IActionResult> ProductByCategory(string name)
        {
            try
            {
                var products = await _productService.GetProductsByCategoryAsync(name);
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
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if(product == null)
                {
                    return NotFound(new { Message = "Product not found" });
                }
                _productService.DeleteProduct(product);
                await _productService.SaveChangesAsync();
                return Ok(new {message= "Product Deleted Successfully" }); 
            }
            catch(Exception ex)
            {
                return StatusCode(500,new {Message = "An error has occured while deleting the product", error = ex.Message });
            }
        }
    }
}
