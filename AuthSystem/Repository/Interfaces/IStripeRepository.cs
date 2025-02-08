using AuthSystem.Models;
using Stripe;
namespace AuthSystem.Repository.Interfaces
{
    public interface IStripeRepository
    {
        Task<string>CreateCheckoutSessionAsync(List<CheckoutItemModel> items);
        //long CalcualteTotalAmount(List<CheckoutItemModel> checkoutItems);
    }
}