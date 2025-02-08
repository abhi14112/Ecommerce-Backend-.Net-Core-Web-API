using AuthSystem.Models;
using AuthSystem.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StripeController:ControllerBase
    {
        private readonly IStripeRepository _stripeService;
        public StripeController(IStripeRepository stripeService)
        {
            _stripeService = stripeService;
        }
        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody]List<CheckoutItemModel>checkoutItems)
        {
            var sessionUrl = await _stripeService.CreateCheckoutSessionAsync(checkoutItems);
            return Ok(new {url = sessionUrl});
        }

    }
}
