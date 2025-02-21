using AuthSystem.Data;
using AuthSystem.Models;
using Stripe.Checkout;
using AuthSystem.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

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
            if (session == null) 
            { 
                return NotFound("Session not found");
            }
            var existingOrder = _context.Orders.FirstOrDefault(o => o.SessionId == session.Id);
            if(existingOrder != null)
            {
                return Ok(existingOrder);
            }
            var productsIds = session.Metadata.ContainsKey("product_ids") ? session.Metadata["product_ids"] : "Unknown";
            var productQuantities = session.Metadata.ContainsKey("quantities") ? session.Metadata["quantities"]:"Unknown";
            var order = new OrderModel
            {
                SessionId = session.Id,
                CustomerEmail = session.CustomerDetails.Email,
                TotalAmount = (decimal)(session.AmountTotal / 100.0),
                PaymentStatus = session.PaymentStatus,
                CreatedAt = DateTime.UtcNow,
                ProductId = productsIds,
                ProductQuantity = productQuantities
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return Ok(order);
        }
        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody]List<CheckoutItemModel>checkoutItems)
        {
            var sessionUrl = await _stripeService.CreateCheckoutSessionAsync(checkoutItems);
            return Ok(new {url = sessionUrl});
        }
    }
}