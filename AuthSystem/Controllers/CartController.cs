using System.Security.Claims;
using AuthSystem.Data;
using AuthSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace AuthSystem.Controllers
{
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        public readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPut("UpdateQuantity/{id}")]
        [Authorize]
        public IActionResult UpdateQuantity(int id,[FromBody] int quantity)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cart = _context.Carts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound(new { message = "Cart not found." });
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (cartItem == null)
            {
                return NotFound(new { message = "Product not found in cart." });
            }
            cartItem.Quantity = quantity;
            _context.SaveChanges();

            return Ok(new { message = "Quantity Updated" , Iquantity=quantity});
        }

        [HttpPost("RemoveFromCart/{id}")]
        [Authorize]
        public IActionResult RemoveFromCart(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var cart = _context.Carts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.UserId == userId);

            if(cart == null)
            {
                return NotFound(new { message = "Cart not found." });
            }
            
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (cartItem == null)
            {
                return NotFound(new { message = "Product not found in cart." });
            }

            cart.Items.Remove(cartItem);
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
            return Ok(new { message = "Product removed from cart successfully" });
        }

        [HttpPost("AddToCart")]
        [Authorize]
        public IActionResult AddToCart([FromBody] AddToCartRequest request)
        {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cart = _context.Carts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
               
                cart = new CartModel
                {
                    UserId = userId,
                    Items = new List<CartItemModel>()
                };

                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

           
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

            if (cartItem != null)
            {
                
                cartItem.Quantity += request.Quantity;
            }
            else
            {
                
                var newCartItem = new CartItemModel
                {
                    CartId = cart.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };
                cart.Items.Add(newCartItem);
            }

           
            _context.SaveChanges();

            return Ok(new
            {
                message = "Product added to cart successfully.",
            });
        }

        [HttpGet("Items")]
        [Authorize]
        public IActionResult GetCartItems()
        {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var cart = _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null || cart.Items == null || !cart.Items.Any())
            {
                return NotFound(new { message = "Cart is empty." });
            }
            return Ok(new
            {
                CartItems = cart.Items.Select(i => new
                {
                    i.ProductId,
                    i.Product.ProductName,
                    i.Quantity,
                    i.Product.Price,
                    i.Product.Image,
                    i.Product.Description,
                    i.Product.Category
                })
            });
        }
    }
}
