using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthSystem.Data;
using AuthSystem.Models;
namespace AuthSystem.Controllers
{

    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("All")]
        public IActionResult GetAllProducts()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }

        [HttpPost("Add")]
        [Authorize(Roles = "admin")]
        public IActionResult AddProduct([FromBody] Product product)
        {
            if(product == null)
            {
                return BadRequest("Product Data is Required");
            }
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return Ok("Product Added Successfully");
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error has occured whle adding the product: {ex.Message}");
            }
        }
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
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
