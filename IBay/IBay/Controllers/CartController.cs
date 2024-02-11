using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace IBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [SwaggerTag("Cart Endpoints")]
    public class CartController(IIbayContext context) : ControllerBase
    {
        [HttpPut("")]
        [SwaggerOperation("Buy a cart")]
        [SwaggerResponse(200, "Cart bought successfully")]
        public IActionResult BuyCart()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                context.BuyCart(userId);
                var cart = context.GetCart(userId);
                return Ok(cart);
            }
            catch(IbayContext.NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(IbayContext.BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }



        [HttpPost("{productId:int}/{quantity:int}")]
        [SwaggerOperation("Add a product to cart")]
        [SwaggerResponse(200, "Product added to cart successfully")]
        public IActionResult AddProductToCart(int productId, int quantity)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                context.AddProductToCart(userId, productId, quantity);
                var cart = context.GetCart(userId);
                return Ok(cart);
            }
            catch(IbayContext.NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(IbayContext.BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }



        [HttpDelete("{productId:int}/{quantity:int}")]
        [SwaggerOperation("Remove a product from cart")]
        [SwaggerResponse(200, "Product removed from cart successfully")]
        public IActionResult RemoveProductFromCart(int productId, int quantity)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                context.RemoveProductFromCart(userId, productId, quantity);
                var cart = context.GetCart(userId);
                return Ok(cart);
            }
            catch(IbayContext.NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(IbayContext.BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }



        [HttpGet("")]
        [SwaggerOperation("Get a cart")]
        [SwaggerResponse(200, "Cart retrieved successfully")]
        public IActionResult GetCart()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var cart = context.GetCart(userId);
                return Ok(cart);
            }
            catch(IbayContext.NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }
    }
}