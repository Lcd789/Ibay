using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController(IIbayContext context) : ControllerBase
    {
        [HttpPut("{userId:int}")]
        public IActionResult BuyCart(int userId)
        {
            var cart = context.BuyCart(userId);
            return Ok(cart);
        }

        [HttpPost("{userId:int}/{productId:int}/{quantity:int}")]
        /// <summary> Adds a product to the cart </summary>
        /// <param name="userId"> The id of the user </param>
        /// <param name="productId"> The id of the product </param>
        /// <param name="quantity"> The quantity of the product </param>
        /// <returns> The updated cart </returns>
        /// <response code="200"> Returns the updated cart </response>
        /// <response code="400"> If the product is not valid </response>
        /// <response code="401"> If the user is not authenticated </response>
        /// <response code="403"> If the user is not authorized </response>
        /// <response code="404"> If the user or the product is not found </response>
        /// <response code="409"> If the product is not available </response>
        public IActionResult AddProductToCart(int userId, int productId, int quantity)
        {
            context.AddProductToCart(userId, productId, quantity);
            return Ok();
        }

        [HttpDelete("{userId:int}/{productId:int}/{quantity:int}")]
        public IActionResult RemoveProductFromCart(int userId, int productId, int quantity)
        {
            var cart = context.RemoveProductFromCart(userId, productId, quantity);
            return Ok(cart);
        }

        [HttpGet("{userId:int}")]
        public IActionResult GetCart(int userId)
        {
            var cart = context.GetCart(userId);
            return Ok(cart);
        }
    }
}
