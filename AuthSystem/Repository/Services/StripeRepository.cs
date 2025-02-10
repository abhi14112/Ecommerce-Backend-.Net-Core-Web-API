using AuthSystem.Models;
using AuthSystem.Repository.Interface;
using Stripe;
using Stripe.Checkout;
namespace AuthSystem.Repository.Services
{
    public class StripeRepository:IStripeRepository
    {
        public readonly string _stripeApiKey;
        public StripeRepository(IConfiguration configuration)
        {
            _stripeApiKey = configuration["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = _stripeApiKey;
        }
        public async Task<string> CreateCheckoutSessionAsync(List<CheckoutItemModel> checkoutItems)
        {
            var domain = "http://localhost:5173";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = checkoutItems.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Name,
                        },
                        UnitAmount = (long)(item.Price * 100)
                    },
                    Quantity = item.Quantity
                }).ToList(),
                Mode = "payment",
                SuccessUrl = $"{domain}/success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{domain}/failed",
                Metadata = new Dictionary<string, string>
                {
                    {"product_ids", String.Join(',',checkoutItems.Select(i => i.ProductId))},
                    {"quantities",String.Join(',',checkoutItems.Select(i => i.Quantity)) }
                }
            };
            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = await service.CreateAsync(options);
            return session.Url;
            }
    }
}