using AuthSystem.Models;
using Microsoft.AspNetCore.Mvc;
namespace AuthSystem.Repository.Interface
{
    public interface ICartRepository
    {
        Task AddCartAsync(CartModel cart);
        Task AddCartItemAsync(CartItemModel cart);
        Task<CartModel> GetCartByUserIdAsync(int userId);
        Task<CartItemModel>GetCartItemByIdAsync(int cartId, int productId);
        Task UpdateCartItemQuantityAsync(CartItemModel item, int quantity);
        Task RemoveCartItemAsync(CartItemModel cartItem);
        Task SaveChangesAsync();

    }
















}