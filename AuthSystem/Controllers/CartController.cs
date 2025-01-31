﻿using System.Security.Claims;
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
               
                cart = new Cart
                {
                    UserId = userId,
                    Items = new List<CartItem>()
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
                
                var newCartItem = new CartItem
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
                cart.TotalPrice 
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
                    i.Id,
                    i.ProductId,
                    ProductName = i.Product.ProductName,
                    i.Quantity,
                    i.Product.Price,
                    i.TotalPrice
                }),
                TotalCartPrice = cart.TotalPrice
            });
        }
    }
}
