using AuthSystem.Models;
using Microsoft.AspNetCore.Mvc;
namespace AuthSystem.Repository.Interface
{
    public interface ICartRepository
    {
        Task<IActionResult>UpdateQuantity(int id, int quantity);
        Task<IActionResult> RemoveFromCart(int id);
        Task<IActionResult> AddToCart(AddToCartRequest item);
        Task<IActionResult> GetCartItem();
    }
}