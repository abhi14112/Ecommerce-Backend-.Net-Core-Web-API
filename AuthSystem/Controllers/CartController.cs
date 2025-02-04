using System.Security.Claims;
using AuthSystem.Data;
using AuthSystem.DTOs;
using AuthSystem.Models;
using AuthSystem.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace AuthSystem.Controllers
{
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        public readonly ICartRepository _cartService;
        public CartController(ICartRepository cartService)
        {
            _cartService = cartService;
        }

        [HttpPut("UpdateQuantity/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateQuantity(int id,[FromBody] int quantity)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound(new { message = "Cart not found." });
            }
            var cartItem = await _cartService.GetCartItemByIdAsync(cart.Id, id);
            if(cartItem == null)
            {
                return NotFound(new { message = "not found in cart." });
            }
            await _cartService.UpdateCartItemQuantityAsync(cartItem, quantity);
            await _cartService.SaveChangesAsync();
            return Ok(new {message = "Quantity Updated",Iquantity = quantity});
        }

        [HttpPost("RemoveFromCart/{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var cart =await _cartService.GetCartByUserIdAsync(userId);

            if(cart == null)
            {
                return NotFound(new { message = "Cart not found." });
            }

            var cartItem = await _cartService.GetCartItemByIdAsync(cart.Id, id);

            if (cartItem == null)
            {
                return NotFound(new { message = "Product not found in cart." });
            }

            cart.Items.Remove(cartItem);
            await _cartService.RemoveCartItemAsync(cartItem);

            await _cartService.SaveChangesAsync();
            return Ok(new { message = "Product removed from cart successfully" });
        }

        [HttpPost("AddToCart")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] QuantityDTO request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cart = await _cartService.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
               
                cart = new CartModel
                {
                    UserId = userId,
                    Items = new List<CartItemModel>()
                };
                await _cartService.AddCartAsync(cart);
                await _cartService.SaveChangesAsync();
            }
            var cartItem = await _cartService.GetCartItemByIdAsync(cart.Id, request.ProductId);
           
            if(cartItem != null)
            {
                await _cartService.UpdateCartItemQuantityAsync(cartItem, request.Quantity);
            }
            else
            {
                
                var newCartItem = new CartItemModel
                {
                    CartId = cart.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };
                await _cartService.AddCartItemAsync(newCartItem);
            }
            await _cartService.SaveChangesAsync();
            return Ok(new
            {
                message = "Product added to cart successfully.",
            });
        }

        [HttpGet("Items")]
        [Authorize]
        public async Task<IActionResult> GetCartItems()
        {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var cart = await _cartService.GetCartByUserIdAsync(userId);


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
