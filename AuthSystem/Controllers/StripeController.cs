using AuthSystem.Data;
using AuthSystem.Models;
using Stripe.Checkout;
using AuthSystem.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AuthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StripeController:ControllerBase
    {
        private readonly IStripeRepository _stripeService;
        private readonly ApplicationDbContext _context;
        public StripeController(IStripeRepository stripeService, ApplicationDbContext context)
        {
            _stripeService = stripeService;
            _context = context;
        }
        [HttpGet("session/{sessionId}")]
        public async Task<IActionResult>GetPaymentDetails(string sessionId)
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);
            Console.WriteLine(session.ToJson());
            if (session == null) 
            { 
                return NotFound("Session not found");
            }
            var existingOrder = _context.Orders.FirstOrDefault(o => o.SessionId == session.Id);
            if(existingOrder != null)
            {
                return Ok(existingOrder);
            }
            Console.WriteLine(session.Metadata);
            var productsIds = session.Metadata.ContainsKey("product_ids") ? session.Metadata["product_ids"] : "Unknown";
            var address = session.Metadata.ContainsKey("address") ? session.Metadata["address"] : "Unknown";
            var productQuantities = session.Metadata.ContainsKey("quantities") ? session.Metadata["quantities"]:"Unknown";
            var order = new OrderModel
            {
                SessionId = session.Id,
                CustomerEmail = session.CustomerDetails.Email,
                TotalAmount = (decimal)(session.AmountTotal / 100.0),
                PaymentStatus = session.PaymentStatus,
                CreatedAt = DateTime.UtcNow,
                ProductId = productsIds,
                ProductQuantity = productQuantities,
                Address = address
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return Ok(order);
        }
        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody]List<CheckoutItemModel>checkoutItems)
        {
            foreach (var item in checkoutItems) 
            {
                Console.Write(JsonConvert.SerializeObject(item));
            }

            var sessionUrl = await _stripeService.CreateCheckoutSessionAsync(checkoutItems);
            return Ok(new {url = sessionUrl});
        }
    }
}