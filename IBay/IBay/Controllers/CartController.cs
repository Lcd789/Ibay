using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace IBay.Controllers
{
    [ApiController]
    [Route("api/[controller]/{userId:int}")]
    [Authorize]
    [SwaggerTag("Cart Endpoints")]
    public class CartController(IIbayContext context) : ControllerBase
    {
        [HttpPut("")]
        [SwaggerOperation("Buy a cart")]
        [SwaggerResponse(200, "Cart bought successfully")]
        public IActionResult BuyCart(int userId)
        {
            var cart = context.BuyCart(userId);
            return Ok(cart);
        }

        [HttpPost("{productId:int}/{quantity:int}")]
        [SwaggerOperation("Add a product to cart")]
        [SwaggerResponse(200, "Product added to cart successfully")]
        public IActionResult AddProductToCart(int userId, int productId, int quantity)
        {
            context.AddProductToCart(userId, productId, quantity);
            return Ok();
        }

        [HttpDelete("{productId:int}/{quantity:int}")]
        [SwaggerOperation("Remove a product from cart")]
        [SwaggerResponse(200, "Product removed from cart successfully")]
        public IActionResult RemoveProductFromCart(int userId, int productId, int quantity)
        {
            var cart = context.RemoveProductFromCart(userId, productId, quantity);
            return Ok(cart);
        }

        [HttpGet("")]
        [SwaggerOperation("Get a cart")]
        [SwaggerResponse(200, "Cart retrieved successfully")]
        public IActionResult GetCart(int userId)
        {
            var cart = context.GetCart(userId);
            return Ok(cart);
        }
    }
}