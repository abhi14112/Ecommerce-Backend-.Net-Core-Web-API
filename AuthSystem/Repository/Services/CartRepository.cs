using AuthSystem.Data;
using AuthSystem.Models;
using AuthSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Repository.Services
{
    public class CartRepository:ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CartModel> GetCartByUserIdAsync(int userId)
        {
            return await  _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
        }
        public async Task<CartItemModel> GetCartItemByIdAsync(int cartId, int productId)
        {
            var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart == null) return null;
            return cart.Items.FirstOrDefault(i => i.ProductId == productId);
        }
        public async Task UpdateCartItemQuantityAsync(CartItemModel cartItem, int quantity)
        {
            cartItem.Quantity += quantity;
            _context.Entry(cartItem).State = EntityState.Modified; 
        }
        public async Task RemoveCartItemAsync(CartItemModel cartItemModel)
        {
            _context.CartItems.Remove(cartItemModel);
        }

        public async Task AddCartAsync(CartModel cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        public async Task AddCartItemAsync(CartItemModel cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
