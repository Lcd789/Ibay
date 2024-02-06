using DAL.Data;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        public IActionResult Create(int sellerId, string productName, string productDescription,
            ProductType productType, double productPrice, int productStock)
        {
            var newProduct = context.CreateProduct(sellerId, productName, productDescription,
                productType, productPrice, productStock);
            return Ok(newProduct);
        }

        [HttpGet]
        [SwaggerOperation("Get all products")]
        [SwaggerResponse(200, "List of all products")]
        public IActionResult Get()
        {
            var products = context.GetProducts();
            return Ok(products);
        }
        
        [HttpGet("/sort")]
        [SwaggerOperation("Get sorted products")]
        [SwaggerResponse(200, "List of sorted products")]
        public IActionResult Get(SortCategory sortCategory, int limit)
        {
            var products = context.GetProductSortedBy(sortCategory, limit);
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

        [HttpPut("{id:int}")]
        [SwaggerOperation("Update a product")]
        [SwaggerResponse(200, "Product updated successfully")]
        [SwaggerResponse(404, "Product not found")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            var updatedProduct = context.UpdateProduct(id, product.ProductName, product.ProductDescription,
                product.ProductType, product.ProductPrice, product.ProductStock, product.Available);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            return Ok(updatedProduct);
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation("Delete a product")]
        [SwaggerResponse(200, "Product deleted successfully")]
        [SwaggerResponse(404, "Product not found")]
        public IActionResult Delete(int id)
        {
            var product = context.DeleteProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
