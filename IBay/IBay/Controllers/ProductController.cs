using DAL.Data;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IBay.AuthorizationMiddleware;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace IBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [SwaggerTag("Product Endpoints")]
    public class ProductController(IIbayContext context) : ControllerBase
    {
        [HttpPost("{sellerId:int}")]
        [SwaggerOperation("Create a new product")]
        [SwaggerResponse(200, "Product created successfully")]
        public IActionResult Create(int sellerId, string productName, string productDescription,ProductType productType, double productPrice, int productStock)
        {
            try
            {
                var newProduct = context.CreateProduct(sellerId, productName, productDescription,
                productType, productPrice, productStock);
                return Ok(newProduct);
            }
            catch(IbayContext.NotFoundException ex){
                return NotFound(ex.Message);
            }
            catch (IbayContext.BadRequestException ex) {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet]
        [SwaggerOperation("Get all products")]
        [SwaggerResponse(200, "List of all products")]
        public IActionResult Get()
        {
            var products = context.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        [SwaggerOperation("Get a product by id")]
        [SwaggerResponse(200, "Product found")]
        [SwaggerResponse(404, "Product not found")]
        public IActionResult GetById(int id)
        {
            var product = context.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("sort")]
        [SwaggerOperation("Get sorted products")]
        [SwaggerResponse(200, "List of sorted products")]
        public IActionResult Get(SortCategory sortCategory, int limit)
        {
            try
            {
                var products = context.GetProductSortedBy(sortCategory, limit);
                return Ok(products);
            }
            catch(IbayContext.BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut("{id:int}")]
        [SwaggerOperation("Update a product")]
        [SwaggerResponse(200, "Product updated successfully")]
        [SwaggerResponse(404, "Product not found")]
        public IActionResult Update(int id, string updatedProductName, string updatedProductDescription, ProductType updatedProductType, double updatedProductPrice, int updatedProductStock, bool updatedProductAvailable )
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var productToCheck = context.GetProductById(id);

            if (userId == null || productToCheck == null)
            {
                return Forbid();
            }
            if(productToCheck.seller.user_id != int.Parse(userId))
            {
                Console.WriteLine("Not product owner");
                return Forbid();
            }

            try
            {
                var updatedProduct = context.UpdateProduct(id, updatedProductName, updatedProductDescription, updatedProductType, updatedProductPrice, updatedProductStock, updatedProductAvailable);
                if (updatedProduct == null)
                {
                    return NotFound();
                }
                return Ok(updatedProduct);
            }
            catch(IbayContext.NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation("Delete a product")]
        [SwaggerResponse(200, "Product deleted successfully")]
        [SwaggerResponse(404, "Product not found")]
        public IActionResult Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var productToCheck = context.GetProductById(id);

            if (userId == null || productToCheck == null)
            {
                return Forbid();
            }
            if (productToCheck.seller.user_id != int.Parse(userId))
            {
                Console.WriteLine("Not product owner");
                return Forbid();
            }

            try
            {
                var product = context.DeleteProduct(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch(IbayContext.NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }
    }
}
